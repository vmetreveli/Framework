using Meadow_Framework.Framework.Abstractions.Events;

namespace Meadow_Framework.Framework.Abstractions.Kernel;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
    Task DispatchAsync(IDomainEvent[] events, CancellationToken cancellationToken = default);
}