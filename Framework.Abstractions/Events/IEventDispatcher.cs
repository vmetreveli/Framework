using Framework.Abstractions.Primitives;

namespace Framework.Abstractions.Events;

/// <summary>
///     Defines a dispatcher for publishing events.
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    ///     Publishes a domain event asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of the domain event to publish.</typeparam>
    /// <param name="event">The domain event to publish.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishDomainEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;

    /// <summary>
    ///     Publishes an integration event asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of the integration event to publish.</typeparam>
    /// <param name="event">The integration event to publish.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IntegrationBaseEvent;
}