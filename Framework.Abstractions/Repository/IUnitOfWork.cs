using System.Data;

namespace Framework.Abstractions.Repository;

/// <summary>
///     Defines the contract for a unit of work that manages transactional operations and persistence of changes.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    ///     Asynchronously commits all changes made in the current unit of work.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously begins a new transaction with the specified isolation level.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <param name="commandLifetime">An optional timespan to set the command timeout for the transaction.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TimeSpan? commandLifetime = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}