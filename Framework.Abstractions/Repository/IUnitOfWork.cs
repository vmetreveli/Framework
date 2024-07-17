using System.Data;

namespace Framework.Abstractions.Repository;

public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TimeSpan? commandLifetime = null, CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}