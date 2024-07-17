using Framework.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Infrastructure.Interceptors;

public sealed class UpdateDeletableEntitiesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateDeletableEntities(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateDeletableEntities(DbContext context)
    {
        var utcNow = DateTime.UtcNow;
        var entries =
            context
                .ChangeTracker
                .Entries<IDeletableEntity>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            entityEntry.State = EntityState.Modified;
            entityEntry.Property(a => a.DeletedOn)
                .CurrentValue = DateTime.UtcNow;
            entityEntry.Property(a => a.IsDeleted)
                .CurrentValue = true;
        }
    }
}