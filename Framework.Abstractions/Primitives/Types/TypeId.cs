namespace Framework.Abstractions.Primitives.Types;

/// <summary>
///     Represents a base class for types identified by a <see cref="Guid" />.
/// </summary>
public abstract class TypeId : IEquatable<TypeId>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TypeId" /> class with the specified <see cref="Guid" />.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    protected TypeId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the identifier value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    ///     Determines whether the current <see cref="TypeId" /> is equal to another <see cref="TypeId" />.
    /// </summary>
    /// <param name="other">The <see cref="TypeId" /> to compare with the current <see cref="TypeId" />.</param>
    /// <returns>true if the current <see cref="TypeId" /> is equal to the other <see cref="TypeId" />; otherwise, false.</returns>
    public bool Equals(TypeId other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Value.Equals(other.Value);
    }

    /// <summary>
    ///     Determines whether the current <see cref="TypeId" /> is an empty <see cref="Guid" />.
    /// </summary>
    /// <returns>true if the identifier value is an empty <see cref="Guid" />; otherwise, false.</returns>
    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((TypeId)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    ///     Implicitly converts a <see cref="TypeId" /> to its <see cref="Guid" /> value.
    /// </summary>
    /// <param name="typeId">The <see cref="TypeId" /> to convert.</param>
    public static implicit operator Guid(TypeId typeId)
    {
        return typeId.Value;
    }

    /// <summary>
    ///     Determines whether two <see cref="TypeId" /> instances are equal.
    /// </summary>
    /// <param name="a">The first <see cref="TypeId" /> instance.</param>
    /// <param name="b">The second <see cref="TypeId" /> instance.</param>
    /// <returns>true if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(TypeId a, TypeId b)
    {
        if (ReferenceEquals(a, b)) return true;

        if (a is not null && b is not null) return a.Value.Equals(b.Value);

        return false;
    }

    /// <summary>
    ///     Determines whether two <see cref="TypeId" /> instances are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="TypeId" /> instance.</param>
    /// <param name="b">The second <see cref="TypeId" /> instance.</param>
    /// <returns>true if the two instances are not equal; otherwise, false.</returns>
    public static bool operator !=(TypeId a, TypeId b)
    {
        return !(a == b);
    }

    /// <summary>
    ///     Returns a string representation of the <see cref="TypeId" />.
    /// </summary>
    /// <returns>A string representing the <see cref="Guid" /> value of the <see cref="TypeId" />.</returns>
    public override string ToString()
    {
        return Value.ToString();
    }
}