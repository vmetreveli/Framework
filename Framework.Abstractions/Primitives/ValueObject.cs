namespace Framework.Abstractions.Primitives;

/// <summary>
///     Represents the base class all value objects derive from.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null) return true;

        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        if (obj is not ValueObject valueObject) return false;

        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    /// <inheritdoc />
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
    /// </summary>
    /// <returns>The collection of objects representing the value object values.</returns>
    public abstract IEnumerable<object> GetAtomicValues();
}