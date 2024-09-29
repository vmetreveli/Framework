namespace Framework.Abstractions.Commands;

/// <summary>
///     Dispatches commands to their respective handlers.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    ///     Sends a command of type <typeparamref name="TCommand" /> asynchronously.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to send.</typeparam>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand;

    /// <summary>
    ///     Sends a command that returns a result of type <typeparamref name="TResult" /> asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the command handler.</typeparam>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with the result of type <typeparamref name="TResult" />.</returns>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}