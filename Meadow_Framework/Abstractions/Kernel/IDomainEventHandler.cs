using Meadow_Framework.Abstractions.Events;

namespace Meadow_Framework.Abstractions.Kernel;

/// <summary>
///     Defines a handler for processing domain events of type <typeparamref name="TEvent" />.
/// </summary>
/// <typeparam name="TEvent">The type of domain event that the handler processes.</typeparam>
public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
{
    /// <summary>
    ///     Handles the specified domain event asynchronously.
    /// </summary>
    /// <param name="event">The domain event to handle.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}