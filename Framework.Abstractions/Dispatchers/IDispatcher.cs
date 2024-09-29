using Framework.Abstractions.Commands;
using Framework.Abstractions.Queries;

namespace Framework.Abstractions.Dispatchers;

/// <summary>
///     Defines the contract for a dispatcher that can handle both commands and queries.
/// </summary>
/// <remarks>
///     This interface combines the responsibilities of both <see cref="ICommandDispatcher" /> and
///     <see cref="IQueryDispatcher" />. Implementations of this interface are expected to
///     support dispatching commands and queries, providing a unified way to handle
///     different types of operations within the application.
/// </remarks>
public interface IDispatcher : ICommandDispatcher, IQueryDispatcher
{
    // This interface does not define any additional members. It inherits the members
    // from both <see cref="ICommandDispatcher"/> and <see cref="IQueryDispatcher"/>,
    // allowing implementations to handle dispatching of commands and queries.
}