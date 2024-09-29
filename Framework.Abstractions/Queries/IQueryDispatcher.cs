namespace Framework.Abstractions.Queries;

/// <summary>
///     Defines the contract for dispatching queries in the system.
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    ///     Dispatches the given query and returns the result asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by handling the query.</typeparam>
    /// <param name="query">The query to dispatch.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResult" />.</returns>
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}