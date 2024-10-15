using Framework.Abstractions.Outbox;
using Framework.Abstractions.Repository;
using Framework.Infrastructure.Context;

// Contains the definitions for outbox-related entities and interfaces
// Contains repository interface definitions

// Provides the BaseDbContext for database operations

namespace Framework.Infrastructure.Repository;

/// <summary>
///     Repository implementation for handling <see cref="OutboxMessage" /> operations in the context of an outbox pattern.
///     Extends the base <see cref="Repository{TDbContext, TEntity, TId}" /> class to work with
///     <see cref="OutboxMessage" />.
/// </summary>
public class OutboxRepository(BaseDbContext context)
    : RepositoryBase<BaseDbContext, OutboxMessage, Guid>(context), IOutboxRepository
{
    /// <summary>
    ///     Creates a new outbox message if a message with the same <see cref="OutboxMessage.EventId" /> doesn't already exist.
    /// </summary>
    /// <param name="outboxMessage">The outbox message to create.</param>
    public async void CreateOutboxMessage(OutboxMessage outboxMessage)
    {
        // Check if an outbox message with the same EventId already exists
        var message = await context
            .OutboxMessages
            .FirstOrDefaultAsync(x =>
                x.EventId == outboxMessage.EventId);

        // If no message exists, add the new outbox message to the context
        if (message is null) context.OutboxMessages.Add(outboxMessage);
    }

    /// <summary>
    ///     Updates the state of an existing outbox message.
    /// </summary>
    /// <param name="eventId">The event ID of the outbox message to update.</param>
    /// <param name="state">The new state to set for the outbox message.</param>
    public async Task UpdateOutboxMessageState(Guid eventId, OutboxMessageState state)
    {
        // Find the outbox message by its event ID
        var outbox = await context.OutboxMessages
            .FirstOrDefaultAsync(m => m.EventId == eventId);

        // If the outbox message exists, change its state
        outbox?.ChangeState(state);
    }

    /// <summary>
    ///     Retrieves all outbox messages that are ready to be sent (i.e., have a state of
    ///     <see cref="OutboxMessageState.ReadyToSend" />).
    /// </summary>
    /// <returns>A list of outbox messages that are ready to be sent.</returns>
    public Task<List<OutboxMessage>> GetAllReadyToSend()
    {
        return context.OutboxMessages
            .Where(m =>
                m.State == OutboxMessageState.ReadyToSend)
            .ToListAsync();
    }

    /// <summary>
    ///     Saves all changes made in this repository's context to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public Task SaveChange()
    {
        return context.SaveChangesAsync();
    }
}