using Meadow_Framework.Core.Abstractions.Primitives;

namespace Meadow_Framework.Core.Infrastructure.Interceptors;

/// <summary>
/// Interceptor for automatically handling soft deletion for entities implementing <see cref="IDeletableEntity"/>.
/// </summary>
/// <remarks>
/// This interceptor intercepts calls to <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>
/// and modifies the state of deleted entities to perform a soft delete instead of a hard delete.
/// </remarks>
public sealed class UpdateDeletableEntitiesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Asynchronously intercepts the saving of changes to the database.
    /// </summary>
    /// <param name="eventData">The event data for the save changes operation.</param>
    /// <param name="result">The current interception result.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="ValueTask{InterceptionResult}"/> representing the asynchronous operation, with the original or modified result.
    /// </returns>
    /// <remarks>
    /// This method identifies entities marked for deletion that implement <see cref="IDeletableEntity"/>
    /// and updates their state to reflect a soft delete.
    /// </remarks>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateDeletableEntities(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the state of deletable entities within the provided <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">The database context containing the entities.</param>
    /// <remarks>
    /// This method filters for entities that implement <see cref="IDeletableEntity"/> and are in the <see cref="EntityState.Deleted"/> state.
    /// It then changes their state to <see cref="EntityState.Modified"/> and sets the <see cref="IDeletableEntity.DeletedOn"/>
    /// and <see cref="IDeletableEntity.IsDeleted"/> properties to perform a soft delete.
    /// </remarks>
    private static void UpdateDeletableEntities(DbContext context)
    {
        var utcNow = DateTime.UtcNow;

        var entries = context.ChangeTracker
            .Entries<IDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            entityEntry.Property(a => a.DeletedOn).CurrentValue = utcNow;
            entityEntry.Property(a => a.IsDeleted).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
        }
    }
}