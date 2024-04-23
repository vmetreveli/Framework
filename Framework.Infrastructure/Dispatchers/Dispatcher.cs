namespace Framework.Infrastructure.Dispatchers;

public sealed class Dispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : IDispatcher
{
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        return await commandDispatcher.SendAsync(command, cancellationToken);
    }


    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return queryDispatcher.QueryAsync(query, cancellationToken);
    }
}