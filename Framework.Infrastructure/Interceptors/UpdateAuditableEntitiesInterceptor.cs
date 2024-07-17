using Framework.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Infrastructure.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        var utcNow = DateTime.UtcNow;
        var entries =
            context
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (var entityEntry in entries)
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    SetCurrentPropertyValue(
                        entityEntry, nameof(IAuditableEntity.CreatedOn), utcNow);
                    SetCurrentPropertyValue(
                        entityEntry, nameof(IAuditableEntity.ModifiedOn), utcNow);
                    break;
                case EntityState.Modified:
                    SetCurrentPropertyValue(
                        entityEntry, nameof(IAuditableEntity.ModifiedOn), utcNow);
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    private static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        DateTime utcNow)
    {
        entry.Property(propertyName).CurrentValue = utcNow;
    }
}