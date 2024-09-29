namespace Framework.Abstractions.Outbox;

/// <summary>
///     Represents the possible states of an outbox message.
/// </summary>
public enum OutboxMessageState
{
    /// <summary>
    ///     The message is ready to be sent.
    /// </summary>
    ReadyToSend = 1,

    /// <summary>
    ///     The message has been sent to the queue.
    /// </summary>
    SendToQueue = 2,

    /// <summary>
    ///     The message has been successfully processed or completed.
    /// </summary>
    Completed = 3
}