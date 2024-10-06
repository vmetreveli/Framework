namespace Framework.Infrastructure.Queries.Dispatcher;

/// <summary>
///     Dispatches queries to the appropriate query handler using the dependency injection service provider.
/// </summary>
public sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    /// <summary>
    ///     Asynchronously dispatches the specified query to the corresponding handler and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The result type expected from the query.</typeparam>
    /// <param name="query">The query to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation, if necessary.</param>
    /// <returns>The result of the query as an asynchronous task.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no valid handler method is found for the query.</exception>
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query,
        CancellationToken cancellationToken = default)
    {
        // Create a scope for resolving dependencies.
        using var scope = serviceProvider.CreateScope();

        // Get the type of the query handler that handles this specific query type and result.
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

        // Resolve the query handler from the service provider.
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        // Retrieve the Handle method from the query handler for this specific query.
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.Handle));

        // If no method is found, throw an exception indicating an invalid handler.
        if (method is null)
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");

        // Invoke the Handle method on the query handler to process the query and return the result.
        return await (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
    }
}