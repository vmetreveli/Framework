namespace Framework.Abstractions.Primitives;

/// <summary>
///     Represents the base class for all entities in the system.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
    where TId : notnull
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityBase{TId}" /> class with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    protected EntityBase(TId id)
    {
        Id = id;
    }

    /// <summary>
    ///     Gets the identifier of the entity.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    ///     Determines whether the specified <see cref="EntityBase{TId}" /> is equal to the current
    ///     <see cref="EntityBase{TId}" />.
    /// </summary>
    /// <param name="other">The <see cref="EntityBase{TId}" /> to compare with the current <see cref="EntityBase{TId}" />.</param>
    /// <returns>
    ///     true if the specified <see cref="EntityBase{TId}" /> is equal to the current <see cref="EntityBase{TId}" />;
    ///     otherwise, false.
    /// </returns>
    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        return Id.Equals(other.Id);
    }

    /// <summary>
    ///     Determines whether two <see cref="EntityBase{TId}" /> instances are equal.
    /// </summary>
    /// <param name="first">The first <see cref="EntityBase{TId}" /> instance.</param>
    /// <param name="second">The second <see cref="EntityBase{TId}" /> instance.</param>
    /// <returns>true if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(EntityBase<TId> first, EntityBase<TId> second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    /// <summary>
    ///     Determines whether two <see cref="EntityBase{TId}" /> instances are not equal.
    /// </summary>
    /// <param name="first">The first <see cref="EntityBase{TId}" /> instance.</param>
    /// <param name="second">The second <see cref="EntityBase{TId}" /> instance.</param>
    /// <returns>true if the two instances are not equal; otherwise, false.</returns>
    public static bool operator !=(EntityBase<TId> first, EntityBase<TId> second)
    {
        return !(first == second);
    }

    /// <summary>
    ///     Determines whether the specified object is equal to the current <see cref="EntityBase{TId}" />.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="EntityBase{TId}" />.</param>
    /// <returns>true if the specified object is equal to the current <see cref="EntityBase{TId}" />; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not EntityBase<TId> entity) return false;
        return Id.Equals(entity.Id);
    }

    /// <summary>
    ///     Returns a hash code for the current <see cref="EntityBase{TId}" />.
    /// </summary>
    /// <returns>A hash code for the current <see cref="EntityBase{TId}" />.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}