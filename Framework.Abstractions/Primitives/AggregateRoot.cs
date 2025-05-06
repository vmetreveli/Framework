using Framework.Abstractions.Events;
using Framework.Abstractions.Primitives.Types;

namespace Framework.Abstractions.Primitives;

// public abstract class AggregateRoot<T>
// {
//     public T Id { get; protected set; }
//     public int Version { get; protected set; } = 1;
//     public IEnumerable<IDomainEvent> Event => _events;
//         
//     private readonly List<IDomainEvent> _events = new();
//     private bool _versionIncremented;
//
//     protected void AddEvent(IDomainEvent @event)
//     {
//         if (!_events.Any() && !_versionIncremented)
//         {
//             Version++;
//             _versionIncremented = true;
//         }
//             
//         _events.Add(@event);
//     }
//
//     public void ClearEvents() => _events.Clear();
//
//     protected void IncrementVersion()
//     {
//         if (_versionIncremented)
//         {
//             return;
//         }
//             
//         Version++;
//         _versionIncremented = true;
//     }
// }
//
// public abstract class AggregateRoot : AggregateRoot<AggregateId>
// {
// }

/// <summary>
///     Represents the base class for aggregate roots with a unique identifier.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class AggregateRoot<TId> : EntityBase<TId>, IAggregateRoot
    where TId : notnull
{
    // A list to keep track of domain events related to this aggregate root.
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="AggregateRoot{TId}" /> class.
    /// </summary>
    /// <param name="id">The unique identifier for the aggregate root.</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the aggregate root.
    /// </summary>
    public new TId Id { get; protected set; }

    /// <summary>
    ///     Gets the collection of domain events associated with this aggregate root.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    /// <summary>
    ///     Clears all domain events from the aggregate root.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    ///     Raises a new domain event and adds it to the list of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}