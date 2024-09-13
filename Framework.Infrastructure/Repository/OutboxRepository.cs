using Framework.Abstractions.Outbox;
using Framework.Abstractions.Repository;
using Framework.Infrastructure.Context;


namespace Framework.Infrastructure.Repository;

public class OutboxRepository(BaseDbContext context)
    : Repository<BaseDbContext, OutboxMessage, Guid>(context), IOutboxRepository
{
    public async void CreateOutboxMessage(OutboxMessage outboxMessage)
    {
        var message = await context
            .OutboxMessages
            .FirstOrDefaultAsync(x =>
                x.EventId == outboxMessage.EventId);
        if (message is null)
        {
            context.OutboxMessages.Add(outboxMessage);
        }
    }

    public async Task UpdateOutboxMesageSatate(Guid eventId, OutboxMessageState state)
    {
        var outbox = await context.OutboxMessages
            .FirstOrDefaultAsync(m => m.EventId == eventId);
        outbox?.ChangeState(state);
    }

    public Task<List<OutboxMessage>> GetAllReadyToSend()
    {
        return context.OutboxMessages
            .Where(m =>
                m.State == OutboxMessageState.ReadyToSend)
            .ToListAsync();
    }

    public Task SaveChange()
    {
        return context.SaveChangesAsync();
    }
}