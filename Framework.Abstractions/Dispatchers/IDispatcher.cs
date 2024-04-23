using Framework.Abstractions.Commands;
using Framework.Abstractions.Queries;

namespace Framework.Abstractions.Dispatchers;

public interface IDispatcher : ICommandDispatcher, IQueryDispatcher
{
}