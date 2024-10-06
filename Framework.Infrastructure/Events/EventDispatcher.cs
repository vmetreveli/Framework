using Framework.Abstractions.Events;
using Framework.Abstractions.Outbox;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Repository;
using MassTransit;

namespace Framework.Infrastructure.Events;

/// <summary>
///     EventDispatcher is responsible for dispatching domain events and integration events.
///     It leverages a service provider to resolve event handlers and a publish endpoint for sending integration events.
/// </summary>
public class EventDispatcher(
    IServiceProvider serviceProvider,
    IPublishEndpoint publisher,
    IOutboxRepository repository,
    IUnitOfWork unitOfWork) : IEventDispatcher
{
    /// <summary>
    ///     Publishes a domain event asynchronously by invoking all registered event handlers for the given event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of domain event being published.</typeparam>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    public async Task PublishDomainEventAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        using var scope = serviceProvider.CreateScope();

        // Resolves all registered event handlers for the event type.
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<IEvent>>();

        // If there are any handlers, execute them asynchronously.
        if (handlers.Any())
        {
            var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken));
            await Task.WhenAll(tasks); // Execute all handler tasks concurrently.
        }
    }

    /// <summary>
    ///     Publishes an integration event asynchronously by sending it to the configured message broker.
    ///     If the event cannot be published, it is stored in the outbox for retrying.
    /// </summary>
    /// <typeparam name="TEvent">The type of integration event being published.</typeparam>
    /// <param name="event">The integration event instance to be published.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    public async Task PublishIntegrationEventAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IntegrationBaseEvent
    {
        try
        {
            // Publish the integration event to the message broker using MassTransit.
            await publisher.Publish(@event, cancellationToken);

            // If the publish succeeds, update the corresponding outbox message state to 'Completed'.
            await repository.UpdateOutboxMessageState(@event.Id, OutboxMessageState.Completed);
        }
        catch (Exception ex)
        {
            // In case of failure, store the event in the outbox for retrying.
            var outboxMessage = new OutboxMessage(@event, @event.Id, @event.CreationDate);
            repository.CreateOutboxMessage(outboxMessage);
        }
        finally
        {
            // Persist changes to the repository (e.g., saving the outbox message).
            await repository.SaveChange();
        }
    }
}