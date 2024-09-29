using Framework.Abstractions.Messaging;

namespace Framework.Abstractions.Commands;

/// <summary>
///     Marker interface for commands in the system.
///     Commands represent requests to perform some action and do not return a result.
/// </summary>
public interface ICommand : IMessage
{
}

/// <summary>
///     Marker interface for commands that return a result.
///     Commands of this type represent requests to perform some action and expect a result of type
///     <typeparamref name="TResponse" />.
/// </summary>
/// <typeparam name="TResponse">The type of the result returned by the command handler.</typeparam>
public interface ICommand<out TResponse> : IMessage
{
}