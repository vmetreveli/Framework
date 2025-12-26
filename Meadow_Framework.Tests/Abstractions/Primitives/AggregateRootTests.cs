using FluentAssertions;
using Meadow_Framework.Core.Abstractions.Events;
using Meadow_Framework.Core.Abstractions.Primitives;
using Moq;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Primitives;

public class AggregateRootTests
{
    private class TestAggregateRoot : AggregateRoot<Guid>
    {
        public TestAggregateRoot(Guid id) : base(id)
        {
        }

        public void DoSomething()
        {
            RaiseDomainEvent(new Mock<IDomainEvent>().Object);
        }
    }

    [Fact]
    public void RaiseDomainEvent_ShouldAddEventToCollection()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Act
        aggregate.DoSomething();

        // Assert
        aggregate.GetDomainEvents().Should().HaveCount(1);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());
        aggregate.DoSomething();

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        aggregate.GetDomainEvents().Should().BeEmpty();
    }
}
