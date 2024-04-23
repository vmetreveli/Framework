using Framework.Abstractions.Messaging;

namespace Framework.Abstractions.Commands;

public interface ICommandHandler<in TCommand> where TCommand : class, IMessage
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : class, IMessage
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}