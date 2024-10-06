using Framework.Abstractions.Messaging;

namespace Framework.Abstractions.Events;

/// <summary>
///     Marker interface for events.
/// </summary>
/// <remarks>
///     This interface is used as a marker to distinguish event messages
///     within the system. It extends the IMessage interface to categorize
///     messages specifically as events.
/// </remarks>
public interface IEvent : IMessage
{
    // This interface does not define any additional members. It serves as a
    // marker interface to identify event messages in the system.
}