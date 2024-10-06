namespace Framework.Abstractions.Primitives;

/// <summary>
///     Represents the base class all value objects derive from.
///     Value objects are compared based on their values rather than their identities.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    ///     Compares this instance with another value object for equality.
    /// </summary>
    /// <param name="other">The other value object to compare with.</param>
    /// <returns>True if the other value object is equal to this instance; otherwise, false.</returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    /// <summary>
    ///     Determines if two value objects are equal.
    /// </summary>
    /// <param name="a">The first value object.</param>
    /// <param name="b">The second value object.</param>
    /// <returns>True if both value objects are equal; otherwise, false.</returns>
    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null) return true;

        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    /// <summary>
    ///     Determines if two value objects are not equal.
    /// </summary>
    /// <param name="a">The first value object.</param>
    /// <param name="b">The second value object.</param>
    /// <returns>True if the value objects are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    /// <summary>
    ///     Checks if this instance is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the object is equal to this instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        if (obj is not ValueObject valueObject) return false;

        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    /// <summary>
    ///     Computes a hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(HashCode), (hashCode, obj) =>
            {
                hashCode.Add(obj.GetHashCode());
                return hashCode;
            }).ToHashCode();
    }

    /// <summary>
    ///     Gets the atomic values of the value object.
    ///     This method should be overridden in derived classes to return the values that make up the value object.
    /// </summary>
    /// <returns>The collection of objects representing the value object values.</returns>
    public abstract IEnumerable<object> GetAtomicValues();
}