using Framework.Abstractions.Primitives;

namespace Framework.Abstractions.Events;

public interface IEventDispatcher
{
    Task PublishDomainEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;

    Task PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IntegrationBaseEvent;
}