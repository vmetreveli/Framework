using Meadow_Framework.Framework.Abstractions.Events;

namespace Meadow_Framework.Framework.Abstractions.Kernel;

public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}