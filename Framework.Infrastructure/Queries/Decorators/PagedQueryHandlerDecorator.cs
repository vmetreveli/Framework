namespace Framework.Infrastructure.Queries.Decorators;

public sealed class PagedQueryHandlerDecorator<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
    : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler = handler;

    // public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    // {
    //     const int maxResults = 100;
    //     const int defaultResults = 10;
    //         
    //     if (query is IPagedQuery pagedQuery)
    //     {
    //         if (pagedQuery.Page <= 0)
    //         {
    //             pagedQuery.Page = 1;
    //         }
    //
    //         if (pagedQuery.Results <= 0)
    //         {
    //             pagedQuery.Results = defaultResults;
    //         }
    //
    //         if (pagedQuery.Results > maxResults)
    //         {
    //             pagedQuery.Results = maxResults;
    //         }
    //     }
    //
    //     return await _handler.HandleAsync(query, cancellationToken);
    // }

    public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}