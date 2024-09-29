namespace Framework.Abstractions.Events;

/// <summary>
///     Defines a handler for processing events of type <typeparamref name="TEvent" />.
/// </summary>
/// <typeparam name="TEvent">The type of event that the handler processes.</typeparam>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    /// <summary>
    ///     Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}