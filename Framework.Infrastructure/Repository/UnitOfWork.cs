using System.Data;
using Framework.Abstractions.Repository;

// Provides access to database transaction isolation levels

// Includes the IUnitOfWork interface definition

namespace Framework.Infrastructure.Repository;

/// <summary>
///     Represents the unit of work for managing database transactions and operations.
///     This class implements <see cref="IUnitOfWork" /> and ensures that changes
///     made to the database context are committed or rolled back as a single unit of work.
/// </summary>
/// <typeparam name="TDbContext">The type of the DbContext that this unit of work manages.</typeparam>
public sealed class UnitOfWork<TDbContext>(TDbContext context) : IUnitOfWork
    where TDbContext : DbContext
{
    /// <summary>
    ///     Asynchronously commits all changes made to the database context.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Disposes of the database context and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this); // Prevents finalizer from running, as context is already disposed
    }

    /// <summary>
    ///     Begins a new database transaction with the specified isolation level and command timeout.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction. Default is ReadCommitted.</param>
    /// <param name="commandLifetime">The command timeout duration. If null, the default timeout is used.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TimeSpan? commandLifetime = null,
        CancellationToken cancellationToken = default)
    {
        if (commandLifetime is not null)
            context.Database.SetCommandTimeout((int)commandLifetime.Value
                .TotalSeconds); // Sets command timeout if specified

        await context.Database.BeginTransactionAsync(isolationLevel,
            cancellationToken); // Begins the transaction with the specified isolation level
    }

    /// <summary>
    ///     Asynchronously commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        var currentTransaction = context.Database.CurrentTransaction;
        if (currentTransaction == null)
            return;

        await currentTransaction.CommitAsync(cancellationToken); // Commits the current transaction
    }

    /// <summary>
    ///     Asynchronously rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        var currentTransaction = context.Database.CurrentTransaction;
        if (currentTransaction == null)
            return;

        await currentTransaction.RollbackAsync(cancellationToken); // Rolls back the current transaction
    }
}