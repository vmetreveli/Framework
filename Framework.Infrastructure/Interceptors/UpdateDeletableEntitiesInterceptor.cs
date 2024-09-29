using Framework.Abstractions.Primitives;

namespace Framework.Infrastructure.Interceptors;

/// <summary>
///     Interceptor for updating entities that implement <see cref="IDeletableEntity" /> before saving changes to the
///     database.
///     This interceptor sets audit fields for soft deletion instead of physically deleting records.
/// </summary>
public sealed class UpdateDeletableEntitiesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    ///     Called asynchronously before changes are saved to the database.
    ///     Updates the deletable entities to perform a soft delete by marking them as deleted.
    /// </summary>
    /// <param name="eventData">The event data associated with the database context.</param>
    /// <param name="result">The current result of the interception.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="ValueTask{InterceptionResult{T}}" /> representing the asynchronous operation.</returns>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // If the context is not null, update the deletable entities
        if (eventData.Context is not null)
            UpdateDeletableEntities(eventData.Context);

        // Call the base method to continue the interception chain
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    ///     Updates entities that implement <see cref="IDeletableEntity" /> which are marked for deletion.
    ///     Performs a soft delete by setting the <see cref="IDeletableEntity.DeletedOn" /> and
    ///     <see cref="IDeletableEntity.IsDeleted" /> properties.
    /// </summary>
    /// <param name="context">The <see cref="DbContext" /> instance to update.</param>
    private static void UpdateDeletableEntities(DbContext context)
    {
        var utcNow = DateTime.UtcNow;

        // Retrieve entries that are marked as deleted
        var entries = context.ChangeTracker
            .Entries<IDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            // Change the entity state from Deleted to Modified to perform a soft delete
            entityEntry.State = EntityState.Modified;

            // Set the DeletedOn property to the current UTC time
            entityEntry.Property(a => a.DeletedOn)
                .CurrentValue = utcNow;

            // Mark the entity as deleted
            entityEntry.Property(a => a.IsDeleted)
                .CurrentValue = true;
        }
    }
}