using Meadow_Framework.Core.Abstractions.Queries;

namespace Meadow_Framework.Core.Infrastructure.Queries.Decorators;

public sealed class PagedQueryHandlerDecorator<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
    : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        const int maxResults = 100;
        const int defaultResults = 10;

        if (query is IPagedQuery pagedQuery)
        {
            if (pagedQuery.Page <= 0) pagedQuery.Page = 1;

            if (pagedQuery.Results <= 0) pagedQuery.Results = defaultResults;

            if (pagedQuery.Results > maxResults) pagedQuery.Results = maxResults;
        }

        return await handler.Handle(query, cancellationToken);
    }
}