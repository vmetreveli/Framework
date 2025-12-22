using Meadow_Framework.Abstractions.Events;

namespace Meadow_Framework.Abstractions.Kernel.Types;

/// <summary>
///     Represents the base class for aggregate roots with versioning and domain event support.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class AggregateRoot<T>
{
    private readonly List<IDomainEvent> _events = new();
    private bool _versionIncremented;

    /// <summary>
    ///     Gets or sets the unique identifier for the aggregate root.
    /// </summary>
    public T Id { get; protected set; }

    /// <summary>
    ///     Gets or sets the version number of the aggregate root for optimistic concurrency control.
    /// </summary>
    public int Version { get; protected set; } = 1;

    /// <summary>
    ///     Gets the collection of domain events associated with this aggregate root.
    /// </summary>
    public IEnumerable<IDomainEvent> Events => _events;

    /// <summary>
    ///     Adds a domain event to the aggregate root and increments the version if this is the first event.
    /// </summary>
    /// <param name="event">The domain event to add.</param>
    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }

        _events.Add(@event);
    }

    /// <summary>
    ///     Clears all domain events from the aggregate root.
    /// </summary>
    public void ClearEvents()
    {
        _events.Clear();
    }

    /// <summary>
    ///     Increments the version of the aggregate root for optimistic concurrency control.
    /// </summary>
    protected void IncrementVersion()
    {
        if (_versionIncremented) return;

        Version++;
        _versionIncremented = true;
    }
}

/// <summary>
///     Represents the base class for aggregate roots with a <see cref="AggregateId" /> identifier.
/// </summary>
public abstract class AggregateRoot : AggregateRoot<AggregateId>
{
}