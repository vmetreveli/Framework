using Framework.Abstractions.Events;

namespace Framework.Abstractions.Primitives.Types;

/// <summary>
///     Represents an aggregate root in the domain model.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    ///     Gets the collection of domain events associated with the aggregate root.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    /// <summary>
    ///     Clears the collection of domain events associated with the aggregate root.
    /// </summary>
    void ClearDomainEvents();
}