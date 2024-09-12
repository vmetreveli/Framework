using Framework.Abstractions.Outbox;
using Framework.Abstractions.Repository;
using OutboxMessage = Framework.Abstractions.Outbox.OutboxMessage;

namespace Framework.Infrastructure.Repository;

public class OutboxRepository(DbContext context)
    : Repository<DbContext, OutboxMessage, Guid>(context), IOutboxRepository
{
    public void CreateOutboxMessage(OutboxMessage outboxMessage)
    {
        context.Set<OutboxMessage>().Add(outboxMessage);
    }

    public async Task UpdateOutboxMesageSatate(Guid eventId, OutboxMessageState state)
    {
        var outbox = await context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(m => m.EventId == eventId);
        outbox.ChangeState(state);
    }

    public Task<List<OutboxMessage>> GetAllReadyToSend()
    {
        return context.Set<OutboxMessage>().Where(m => m.State == OutboxMessageState.ReadyToSend).ToListAsync();
    }

    public Task SaveChange()
    {
        return context.SaveChangesAsync();
    }
}