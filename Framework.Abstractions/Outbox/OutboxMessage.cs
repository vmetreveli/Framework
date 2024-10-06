using Framework.Abstractions.Primitives;
using Newtonsoft.Json;

namespace Framework.Abstractions.Outbox;

/// <summary>
///     Represents an outbox message for handling asynchronous operations.
/// </summary>
public sealed class OutboxMessage : AggregateRoot<Guid>
{
    // Private constructor for EF Core or other serialization mechanisms
    private OutboxMessage() : base(Guid.NewGuid())
    {
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="OutboxMessage" /> class.
    /// </summary>
    /// <param name="message">The message to be stored in the outbox.</param>
    /// <param name="eventId">The identifier for the event.</param>
    /// <param name="eventDate">The date when the event occurred.</param>
    public OutboxMessage(object message, Guid eventId, DateTime eventDate) : base(Guid.NewGuid())
    {
        // Serialize the message to store it as a JSON string
        Data = JsonConvert.SerializeObject(message);

        // Store the type of the message for deserialization later
        Type = message.GetType().FullName + ", " +
               message.GetType().Assembly.GetName().Name;

        // Initialize properties
        EventId = eventId;
        EventDate = eventDate;
        State = OutboxMessageState.ReadyToSend;
        ModifiedDate = DateTime.UtcNow;
    }

    /// <summary>
    ///     Gets the serialized message data.
    /// </summary>
    public string Data { get; }

    /// <summary>
    ///     Gets the full name and assembly name of the message type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for the event associated with the message.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    ///     Gets or sets the date when the event occurred.
    /// </summary>
    public DateTime EventDate { get; set; }

    /// <summary>
    ///     Gets or sets the state of the outbox message.
    /// </summary>
    public OutboxMessageState State { get; private set; }

    /// <summary>
    ///     Gets or sets the date when the outbox message was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public string Content { get; set; }

    /// <summary>
    ///     Changes the state of the outbox message and updates the modification date.
    /// </summary>
    /// <param name="state">The new state of the outbox message.</param>
    public void ChangeState(OutboxMessageState state)
    {
        State = state;
        ModifiedDate = DateTime.UtcNow;
    }

    /// <summary>
    ///     Recreates the original message object from the stored data.
    /// </summary>
    /// <returns>The deserialized message object.</returns>
    public dynamic? RecreateMessage()
    {
        return JsonConvert.DeserializeObject(Data, System.Type.GetType(Type));
    }
}