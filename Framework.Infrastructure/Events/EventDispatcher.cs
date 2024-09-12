using Framework.Abstractions.Events;
using Framework.Abstractions.Outbox;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Repository;
using MassTransit;

namespace Framework.Infrastructure.Events;

public class EventDispatcher(
    IServiceProvider serviceProvider,
    IPublishEndpoint publisher,
    IOutboxRepository repository,
    IUnitOfWork unitOfWork) : IEventDispatcher
{
    public async Task PublishDomainEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<IEvent>>();
        if (handlers.Any())
        {
            var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken));
            await Task.WhenAll(tasks);
        }
    }

    public async Task PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        try
        {
          await publisher.Publish(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            var outboxMessage = new OutboxMessage(@event, @event.Id, @event.CreationDate);
            repository.CreateOutboxMessage(outboxMessage);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}