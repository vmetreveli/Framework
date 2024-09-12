using Framework.Abstractions.Events;

namespace Framework.Abstractions.Kernel.Types;

public abstract class AggregateRoot<T>
{
    private readonly List<IDomainEvent> _events = new();
    private bool _versionIncremented;
    public T Id { get; protected set; }
    public int Version { get; protected set; } = 1;
    public IEnumerable<IDomainEvent> Events => _events;

    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }

        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void IncrementVersion()
    {
        if (_versionIncremented) return;

        Version++;
        _versionIncremented = true;
    }
}

public abstract class AggregateRoot : AggregateRoot<AggregateId>
{
}