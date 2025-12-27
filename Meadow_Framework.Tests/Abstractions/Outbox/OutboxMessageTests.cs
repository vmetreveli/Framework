using FluentAssertions;
using Meadow_Framework.Core.Abstractions.Outbox;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Outbox;

public class OutboxMessageTests
{
    public class TestMessage
    {
        public string Content { get; set; }
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var message = new TestMessage { Content = "Hello" };
        var eventId = Guid.NewGuid();
        var eventDate = DateTime.UtcNow;

        // Act
        var outboxMessage = new OutboxMessage(message, eventId, eventDate);

        // Assert
        outboxMessage.EventId.Should().Be(eventId);
        outboxMessage.EventDate.Should().Be(eventDate);
        outboxMessage.State.Should().Be(OutboxMessageState.ReadyToSend);
        outboxMessage.Data.Should().Contain("Hello");
        outboxMessage.Type.Should().Contain("TestMessage");
    }

    /// <summary>
    ///
    /// </summary>
    [Fact]
    public void ChangeState_ShouldUpdateStateAndModifiedDate()
    {
        // Arrange
        var message = new TestMessage { Content = "Hello" };
        var outboxMessage = new OutboxMessage(message, Guid.NewGuid(), DateTime.UtcNow);
        var initialModifiedDate = outboxMessage.ModifiedDate;

        // Act
        outboxMessage.ChangeState(OutboxMessageState.SendToQueue);

        // Assert
        outboxMessage.State.Should().Be(OutboxMessageState.SendToQueue);
        outboxMessage.ModifiedDate.Should().BeAfter(initialModifiedDate);
    }

    [Fact]
    public void RecreateMessage_ShouldReturnOriginalMessage()
    {
        // Arrange
        var message = new TestMessage { Content = "Hello" };
        var outboxMessage = new OutboxMessage(message, Guid.NewGuid(), DateTime.UtcNow);

        // Act
        var recreatedMessage = outboxMessage.RecreateMessage();

        // Assert
        ((string)recreatedMessage.Content).Should().Be("Hello");
    }
}
