namespace Framework.Abstractions.Events;

public interface IIntegrationEvent : IEvent
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
}