using Framework.Abstractions.Outbox;
using OutboxMessage = Framework.Abstractions.Outbox.OutboxMessage;

namespace Framework.Abstractions.Repository;

public interface IOutboxRepository : IRepository<OutboxMessage, Guid>
{
    void CreateOutboxMessage(OutboxMessage outboxMessage);
    Task UpdateOutboxMesageSatate(Guid eventId, OutboxMessageState state);
    Task<List<OutboxMessage>> GetAllReadyToSend();
    Task SaveChange();
}