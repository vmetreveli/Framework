using Framework.Abstractions.Events;

namespace Framework.Abstractions.Primitives;

/// <summary>
///     Represents the base class for integration events.
///     Integration events are used to communicate across different systems or components.
/// </summary>
public abstract class IntegrationBaseEvent : IEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IntegrationBaseEvent" /> class.
    /// </summary>
    /// <param name="id">The unique identifier of the event.</param>
    /// <param name="createDate">The date and time when the event was created.</param>
    protected IntegrationBaseEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="IntegrationBaseEvent" /> class with default values.
    ///     This constructor generates a new unique identifier and sets the creation date to the current UTC time.
    /// </summary>
    protected IntegrationBaseEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    /// <summary>
    ///     Gets the unique identifier of the event.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    ///     Gets the date and time when the event was created.
    /// </summary>
    public DateTime CreationDate { get; }
}