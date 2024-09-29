namespace Framework.Abstractions.Messaging;

/// <summary>
///     Represents a message with a response type.
/// </summary>
/// <typeparam name="TResponse">The type of the response expected from the message.</typeparam>
public interface IMessage<out TResponse>
{
}

/// <summary>
///     Marker interface for messages without a specific response type.
/// </summary>
public interface IMessage
{
}