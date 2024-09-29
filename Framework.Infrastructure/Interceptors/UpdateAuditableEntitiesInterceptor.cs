using Framework.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Infrastructure.Interceptors;

/// <summary>
///     Interceptor for updating auditable entities before saving changes to the database.
///     Inherits from <see cref="SaveChangesInterceptor" /> and implements auditable entity updates.
/// </summary>
public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    ///     Called asynchronously before changes are saved to the database.
    ///     Updates the audit properties of entities that implement <see cref="IAuditableEntity" />.
    /// </summary>
    /// <param name="eventData">The event data associated with the database context.</param>
    /// <param name="result">The current result of the interception.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="ValueTask{InterceptionResult{T}}" /> representing the asynchronous operation.</returns>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // If the context is not null, update the auditable entities
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        // Call the base method to continue the interception chain
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    ///     Updates the audit properties of entities that implement <see cref="IAuditableEntity" />.
    ///     Sets the <see cref="IAuditableEntity.CreatedOn" /> and <see cref="IAuditableEntity.ModifiedOn" /> properties.
    /// </summary>
    /// <param name="context">The <see cref="DbContext" /> instance to update.</param>
    private static void UpdateAuditableEntities(DbContext context)
    {
        var utcNow = DateTime.UtcNow;
        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        // Iterate over the entries to update audit properties based on entity state
        foreach (var entityEntry in entries)
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    // Set CreatedOn and ModifiedOn for newly added entities
                    SetCurrentPropertyValue(entityEntry, nameof(IAuditableEntity.CreatedOn), utcNow);
                    SetCurrentPropertyValue(entityEntry, nameof(IAuditableEntity.ModifiedOn), utcNow);
                    break;
                case EntityState.Modified:
                    // Set ModifiedOn for modified entities
                    SetCurrentPropertyValue(entityEntry, nameof(IAuditableEntity.ModifiedOn), utcNow);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    // No action needed for detached, unchanged, or deleted entities
                    break;
                default:
                    // Throw an exception if the entity state is not recognized
                    throw new ArgumentOutOfRangeException();
            }
    }

    /// <summary>
    ///     Sets the current value of a property on the entity entry.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry" /> for the entity.</param>
    /// <param name="propertyName">The name of the property to set.</param>
    /// <param name="utcNow">The value to set on the property (typically the current UTC time).</param>
    private static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTime utcNow)
    {
        entry.Property(propertyName).CurrentValue = utcNow;
    }
}