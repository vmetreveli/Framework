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

public abstract class AggregateRoot<TId> : EntityBase<TId>, IAggregateRoot
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];


    protected AggregateRoot(TId id) : base(id)
    {
    }

    public TId Id { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return [.. _domainEvents];
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}