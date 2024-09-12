using System.Data;
using Framework.Abstractions.Repository;

namespace Framework.Infrastructure.Repository;

public sealed class UnitOfWork<TDbContext>(TDbContext context) : IUnitOfWork
    where TDbContext : DbContext
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TimeSpan? commandLifetime = null, CancellationToken cancellationToken = default)
    {
        if (commandLifetime is not null)
            context.Database.SetCommandTimeout((int)commandLifetime.Value.TotalSeconds);

        await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        var currentTransaction = context.Database.CurrentTransaction;
        if (currentTransaction == null)
            return;

        await currentTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        var currentTransaction = context.Database.CurrentTransaction;
        if (currentTransaction == null)
            return;

        await currentTransaction.RollbackAsync(cancellationToken);
    }
}