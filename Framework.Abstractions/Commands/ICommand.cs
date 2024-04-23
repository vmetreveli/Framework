using Framework.Abstractions.Messaging;

namespace Framework.Abstractions.Commands;

// public interface ICommand : IMessage
// {
// }

public interface ICommand : IMessage
{
}

public interface ICommand<out TResponse> : IMessage
{
}