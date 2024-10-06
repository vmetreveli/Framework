namespace Framework.Abstractions.Primitives;

/// <summary>
///     Defines an entity that can be marked as deleted.
///     This interface is used to implement soft deletion of entities, where the entity is not removed from the database
///     but marked as deleted.
/// </summary>
public interface IDeletableEntity
{
    /// <summary>
    ///     Gets a value indicating whether the entity has been marked as deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    ///     Gets the date and time when the entity was marked as deleted.
    /// </summary>
    DateTime? DeletedOn { get; }
}