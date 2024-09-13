using Framework.Abstractions.Primitives;
using Newtonsoft.Json;

namespace Framework.Abstractions.Outbox;

public sealed class OutboxMessage : AggregateRoot<Guid>
{
    private OutboxMessage() : base(Guid.NewGuid())
    {
    }

    public OutboxMessage(object message, Guid eventId, DateTime eventDate) : base(Guid.NewGuid())
    {
        Data = JsonConvert.SerializeObject(message);
        Type = message.GetType().FullName + ", " +
               message.GetType().Assembly.GetName().Name;
        EventId = eventId;
        EventDate = eventDate;
        State = OutboxMessageState.ReadyToSend;
        ModifiedDate = DateTime.UtcNow;
    }

    public string Data { get; }
    public string Type { get; }
    public Guid EventId { get; private set; }
    public DateTime EventDate { get; private set; }
    public OutboxMessageState State { get; private set; }
    public DateTime ModifiedDate { get; set; }

    public void ChangeState(OutboxMessageState state)
    {
        State = state;
        ModifiedDate = DateTime.UtcNow;
    }

    public dynamic? RecreateMessage()
    {
        return JsonConvert.DeserializeObject(Data, System.Type.GetType(Type));
    }
}