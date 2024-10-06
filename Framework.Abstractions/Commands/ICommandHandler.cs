using Framework.Abstractions.Messaging;

namespace Framework.Abstractions.Commands;

/// <summary>
///     Handles a command of type <typeparamref name="TCommand" /> with no return value.
/// </summary>
/// <typeparam name="TCommand">The type of the command to handle.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : class, IMessage
{
    /// <summary>
    ///     Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
///     Handles a command of type <typeparamref name="TCommand" /> and returns a result of type
///     <typeparamref name="TResult" />.
/// </summary>
/// <typeparam name="TCommand">The type of the command to handle.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the command handler.</typeparam>
public interface ICommandHandler<in TCommand, TResult> where TCommand : class, IMessage
{
    /// <summary>
    ///     Handles the specified command asynchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with the result of type <typeparamref name="TResult" />.</returns>
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}