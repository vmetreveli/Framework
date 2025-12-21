namespace Meadow_Framework.Framework.Abstractions.Kernel.Types;

/// <summary>
///     Represents an identifier for an aggregate with a generic value type.
/// </summary>
/// <typeparam name="T">The type of the identifier value.</typeparam>
public class AggregateId<T> : IEquatable<AggregateId<T>>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AggregateId{T}" /> class.
    /// </summary>
    /// <param name="value">The unique identifier value.</param>
    public AggregateId(T value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the identifier value.
    /// </summary>
    public T Value { get; }

    public bool Equals(AggregateId<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AggregateId<T>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Value);
    }
}

/// <summary>
///     Represents an identifier for an aggregate with a <see cref="Guid" /> value.
/// </summary>
public class AggregateId : AggregateId<Guid>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AggregateId" /> class with a new <see cref="Guid" />.
    /// </summary>
    public AggregateId() : this(Guid.NewGuid())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AggregateId" /> class with the specified <see cref="Guid" /> value.
    /// </summary>
    /// <param name="value">The unique identifier value.</param>
    public AggregateId(Guid value) : base(value)
    {
    }

    public static implicit operator Guid(AggregateId id)
    {
        return id.Value;
    }

    public static implicit operator AggregateId(Guid id)
    {
        return new AggregateId(id);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}