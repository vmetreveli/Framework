namespace Framework.Abstractions.Primitives;

public abstract class BaseEvent(Guid id, DateTime createDate)
{
    public BaseEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public Guid Id { get; } = id;

    public DateTime CreationDate { get; } = createDate;
}