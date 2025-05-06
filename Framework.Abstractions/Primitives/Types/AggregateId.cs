namespace Framework.Abstractions.Primitives.Types;

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

    /// <summary>
    ///     Determines whether the specified <see cref="AggregateId{T}" /> is equal to the current instance.
    /// </summary>
    /// <param name="other">The <see cref="AggregateId{T}" /> to compare with the current instance.</param>
    /// <returns>True if the specified <see cref="AggregateId{T}" /> is equal to the current instance; otherwise, false.</returns>
    public bool Equals(AggregateId<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    /// <summary>
    ///     Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>True if the specified object is equal to the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AggregateId<T>)obj);
    }

    /// <summary>
    ///     Gets the hash code for the current instance.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
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

    /// <summary>
    ///     Implicitly converts an <see cref="AggregateId" /> to a <see cref="Guid" />.
    /// </summary>
    /// <param name="id">The <see cref="AggregateId" /> instance to convert.</param>
    /// <returns>The <see cref="Guid" /> value of the <see cref="AggregateId" />.</returns>
    public static implicit operator Guid(AggregateId id)
    {
        return id.Value;
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Guid" /> to an <see cref="AggregateId" />.
    /// </summary>
    /// <param name="id">The <see cref="Guid" /> value to convert.</param>
    /// <returns>An <see cref="AggregateId" /> instance with the specified value.</returns>
    public static implicit operator AggregateId(Guid id)
    {
        return new AggregateId(id);
    }

    /// <summary>
    ///     Returns a string representation of the <see cref="AggregateId" />.
    /// </summary>
    /// <returns>A string representation of the <see cref="AggregateId" />.</returns>
    public override string ToString()
    {
        return Value.ToString();
    }
}