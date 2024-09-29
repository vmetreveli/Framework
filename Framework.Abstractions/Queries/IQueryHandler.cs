namespace Framework.Abstractions.Queries;

/// <summary>
///     Defines the contract for handling queries in the system.
/// </summary>
/// <typeparam name="TQuery">The type of the query to handle.</typeparam>
/// <typeparam name="TResult">The type of the result produced by handling the query.</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    /// <summary>
    ///     Handles the given query and returns the result asynchronously.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResult" />.</returns>
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}