namespace Framework.Infrastructure.Dispatchers;

/// <summary>
///     Dispatcher is a unified dispatcher that handles both commands and queries.
///     It delegates command and query handling to the respective command and query dispatchers.
/// </summary>
public sealed class Dispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : IDispatcher
{
    /// <summary>
    ///     Sends a command asynchronously to the command dispatcher.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command being sent.</typeparam>
    /// <param name="command">The command instance to send.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
    }

    /// <summary>
    ///     Sends a command that returns a result asynchronously to the command dispatcher.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
    /// <param name="command">The command instance to send.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of type <typeparamref name="TResult" />.</returns>
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        return await commandDispatcher.SendAsync(command, cancellationToken);
    }

    /// <summary>
    ///     Sends a query asynchronously to the query dispatcher and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
    /// <param name="query">The query instance to send.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of type <typeparamref name="TResult" />.</returns>
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return queryDispatcher.QueryAsync(query, cancellationToken);
    }
}