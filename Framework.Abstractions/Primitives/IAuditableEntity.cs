namespace Framework.Abstractions.Primitives;

/// <summary>
///     Defines an entity that supports auditing by tracking creation and modification timestamps.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    ///     Gets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedOn { get; }

    /// <summary>
    ///     Gets the date and time when the entity was last modified.
    /// </summary>
    DateTime ModifiedOn { get; }
}