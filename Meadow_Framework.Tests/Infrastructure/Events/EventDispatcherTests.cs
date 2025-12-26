using FluentAssertions;
using MassTransit;
using Meadow_Framework.Core.Abstractions.Events;
using Meadow_Framework.Core.Abstractions.Outbox;
using Meadow_Framework.Core.Abstractions.Primitives;
using Meadow_Framework.Core.Abstractions.Repository;
using Meadow_Framework.Core.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Meadow_Framework.Tests.Infrastructure.Events;

public class EventDispatcherTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<IOutboxRepository> _outboxRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EventDispatcher _eventDispatcher;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;

    public EventDispatcherTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _outboxRepositoryMock = new Mock<IOutboxRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _serviceScopeMock = new Mock<IServiceScope>();

        _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_serviceScopeFactoryMock.Object);
        _serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider)
            .Returns(_serviceProviderMock.Object);

        _eventDispatcher = new EventDispatcher(
            _serviceProviderMock.Object,
            _publishEndpointMock.Object,
            _outboxRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    public class TestDomainEvent : IDomainEvent
    {
    }

    public class TestIntegrationEvent : IntegrationBaseEvent
    {
        public TestIntegrationEvent(Guid id, DateTime creationDate) : base(id, creationDate)
        {
        }
    }

    [Fact]
    public async Task PublishDomainEventAsync_ShouldInvokeHandlers()
    {
        // Arrange
        var domainEvent = new TestDomainEvent();
        var handlerMock = new Mock<IEventHandler<IEvent>>();
        
        // Setup service provider to return the handler
        _serviceProviderMock.Setup(x => x.GetService(typeof(IEnumerable<IEventHandler<IEvent>>)))
            .Returns(new List<IEventHandler<IEvent>> { handlerMock.Object });

        // Act
        await _eventDispatcher.PublishDomainEventAsync(domainEvent);

        // Assert
        handlerMock.Verify(x => x.HandleAsync(domainEvent, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishIntegrationEventAsync_ShouldPublishAndCompleteOutboxMessage_WhenSuccessful()
    {
        // Arrange
        var integrationEvent = new TestIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow);

        // Act
        await _eventDispatcher.PublishIntegrationEventAsync(integrationEvent);

        // Assert
        _publishEndpointMock.Verify(x => x.Publish(integrationEvent, It.IsAny<CancellationToken>()), Times.Once);
        _outboxRepositoryMock.Verify(x => x.UpdateOutboxMessageState(integrationEvent.Id, OutboxMessageState.Completed), Times.Once);
        _outboxRepositoryMock.Verify(x => x.SaveChange(), Times.Once);
    }

    [Fact]
    public async Task PublishIntegrationEventAsync_ShouldCreateOutboxMessage_WhenPublishFails()
    {
        // Arrange
        var integrationEvent = new TestIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow);
        _publishEndpointMock.Setup(x => x.Publish(integrationEvent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Publish failed"));

        // Act
        await _eventDispatcher.PublishIntegrationEventAsync(integrationEvent);

        // Assert
        _outboxRepositoryMock.Verify(x => x.CreateOutboxMessage(It.Is<OutboxMessage>(m => m.EventId == integrationEvent.Id)), Times.Once);
        _outboxRepositoryMock.Verify(x => x.SaveChange(), Times.Once);
    }
}
