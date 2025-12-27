using Meadow_Framework.Core.Abstractions.Events;
using Meadow_Framework.Core.Abstractions.Outbox;
using Meadow_Framework.Core.Abstractions.Primitives;
using Meadow_Framework.Core.Abstractions.Repository;
using Meadow_Framework.Core.Infrastructure.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Quartz;
using Xunit;

namespace Meadow_Framework.Tests.Infrastructure.Jobs;

public class OutboxJobTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IEventDispatcher> _eventDispatcherMock;
    private readonly Mock<IOutboxRepository> _outboxRepositoryMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly OutboxJob _outboxJob;

    public OutboxJobTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _eventDispatcherMock = new Mock<IEventDispatcher>();
        _outboxRepositoryMock = new Mock<IOutboxRepository>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _serviceScopeMock = new Mock<IServiceScope>();

        _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_serviceScopeFactoryMock.Object);
        _serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider)
            .Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IOutboxRepository)))
            .Returns(_outboxRepositoryMock.Object);

        _outboxJob = new OutboxJob(_serviceProviderMock.Object, _eventDispatcherMock.Object);
    }

    public class TestIntegrationEvent : IntegrationBaseEvent
    {
        public TestIntegrationEvent(Guid id, DateTime creationDate) : base(id, creationDate)
        {
        }
    }

    [Fact]
    public async Task Execute_ShouldPublishReadyToSendMessages()
    {
        // Arrange
        var integrationEvent = new TestIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow);
        var outboxMessage = new OutboxMessage(integrationEvent, integrationEvent.Id, integrationEvent.CreationDate);
        
        _outboxRepositoryMock.Setup(x => x.GetAllReadyToSend())
            .ReturnsAsync(new List<OutboxMessage> { outboxMessage });

        var jobExecutionContextMock = new Mock<IJobExecutionContext>();

        // Act
        await _outboxJob.Execute(jobExecutionContextMock.Object);

        // Assert
        _eventDispatcherMock.Verify(x => x.PublishIntegrationEventAsync(It.IsAny<TestIntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
