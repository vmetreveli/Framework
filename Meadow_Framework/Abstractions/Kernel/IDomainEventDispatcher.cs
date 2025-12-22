using Meadow_Framework.Abstractions.Events;

namespace Meadow_Framework.Abstractions.Kernel;

/// <summary>
///     Defines the contract for dispatching domain events.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    ///     Asynchronously dispatches a single domain event.
    /// </summary>
    /// <param name="event">The domain event to dispatch.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(IDomainEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously dispatches multiple domain events.
    /// </summary>
    /// <param name="events">The array of domain events to dispatch.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(IDomainEvent[] events, CancellationToken cancellationToken = default);
}