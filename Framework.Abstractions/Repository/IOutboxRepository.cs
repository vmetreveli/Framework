using Framework.Abstractions.Outbox;
using OutboxMessage = Framework.Abstractions.Outbox.OutboxMessage;

namespace Framework.Abstractions.Repository;

/// <summary>
///     Defines the contract for a repository that handles outbox messages.
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    ///     Creates a new outbox message in the repository.
    /// </summary>
    /// <param name="outboxMessage">The outbox message to create.</param>
    void CreateOutboxMessage(OutboxMessage outboxMessage);

    /// <summary>
    ///     Asynchronously updates the state of an outbox message with the specified event ID.
    /// </summary>
    /// <param name="eventId">The identifier of the event associated with the outbox message.</param>
    /// <param name="state">The new state to set for the outbox message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateOutboxMessageState(Guid eventId, OutboxMessageState state);

    /// <summary>
    ///     Asynchronously retrieves all outbox messages that are ready to be sent.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a result of a list of outbox messages ready to be sent.</returns>
    Task<List<OutboxMessage>> GetAllReadyToSend();

    /// <summary>
    ///     Asynchronously saves changes made to the outbox repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveChange();
}