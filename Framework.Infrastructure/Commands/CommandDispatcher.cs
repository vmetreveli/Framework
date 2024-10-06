namespace Framework.Infrastructure.Commands;

/// <summary>
///     The command dispatcher class responsible for dispatching commands to the appropriate handlers.
///     Implements <see cref="ICommandDispatcher" />.
/// </summary>
/// <param name="serviceProvider">The <see cref="IServiceProvider" /> used to resolve dependencies.</param>
public sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    /// <summary>
    ///     Dispatches a command of type <typeparamref name="TCommand" /> to its corresponding handler.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to dispatch.</typeparam>
    /// <param name="command">The command instance to be handled.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        // Creates a new dependency injection scope to resolve the command handler
        using var scope = serviceProvider.CreateScope();

        // Resolves the command handler for the provided command type
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        // Invokes the handler's Handle method to process the command
        await handler.Handle(command, cancellationToken);
    }

    /// <summary>
    ///     Dispatches a command of type <typeparamref name="TCommand" /> and returns a result of type
    ///     <typeparamref name="TResult" />.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to dispatch.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
    /// <param name="command">The command instance to be handled.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the result of the asynchronous operation.</returns>
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        // Creates a new dependency injection scope to resolve the command handler
        using var scope = serviceProvider.CreateScope();

        // Resolves the handler type dynamically based on the command and result types
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

        // Resolves the command handler
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        // Retrieves the Handle method from the resolved handler type
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand, TResult>.Handle));

        // Throws an exception if the handler does not contain a valid Handle method
        if (method is null)
            throw new InvalidOperationException($"Command handler for '{typeof(TResult).Name}' is invalid.");

        // Invokes the handler's Handle method and returns the result
        // ReSharper disable once PossibleNullReferenceException
        return await (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken });
    }
}