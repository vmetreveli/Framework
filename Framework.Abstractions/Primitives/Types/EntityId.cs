namespace Framework.Abstractions.Primitives.Types;

/// <summary>
///     Represents an identifier for an entity.
/// </summary>
public class EntityId : TypeId
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityId" /> class.
    /// </summary>
    /// <param name="value">The unique identifier value.</param>
    public EntityId(Guid value) : base(value)
    {
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Guid" /> to an <see cref="EntityId" />.
    /// </summary>
    /// <param name="id">The <see cref="Guid" /> value to convert.</param>
    /// <returns>An <see cref="EntityId" /> instance with the specified value.</returns>
    public static implicit operator EntityId(Guid id)
    {
        return new EntityId(id);
    }

    /// <summary>
    ///     Implicitly converts an <see cref="EntityId" /> to a <see cref="Guid" />.
    /// </summary>
    /// <param name="id">The <see cref="EntityId" /> instance to convert.</param>
    /// <returns>The <see cref="Guid" /> value of the <see cref="EntityId" />.</returns>
    public static implicit operator Guid(EntityId id)
    {
        return id.Value;
    }
}