namespace Framework.Infrastructure.Commands;

public sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.Handle(command, cancellationToken);
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand, TResult>.Handle));
        if (method is null)
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");

        // ReSharper disable once PossibleNullReferenceException
        return await (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken });
    }
}