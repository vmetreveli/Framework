namespace Framework.Abstractions.Events;

/// <summary>
///     Marker interface for domain events.
/// </summary>
/// <remarks>
///     This interface extends the <see cref="IEvent" /> interface to specifically
///     identify domain events within the system. Domain events are events that
///     occur as a result of a domain operation and are used to communicate state
///     changes or trigger additional business logic within the domain.
/// </remarks>
public interface IDomainEvent : IEvent
{
    // This interface does not define any additional members. It serves as a
    // marker interface to identify domain events, which are a subset of events
    // that are specifically related to changes or operations within the domain.
}