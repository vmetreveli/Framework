namespace Framework.Infrastructure.Queries.Dispatcher;

public sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.Handle));
        if (method is null)
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");

        // ReSharper disable once PossibleNullReferenceException
        return await (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
    }
}