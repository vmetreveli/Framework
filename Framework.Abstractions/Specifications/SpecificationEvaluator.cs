using Framework.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Framework.Abstractions.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TId>(
        IQueryable<TEntity> inputQueryable, Specification<TEntity, TId> specification)
        where TEntity : EntityBase<TId>
        where TId : notnull
    {
        var queryable = inputQueryable;

        if (specification.Criteria is not null)
            queryable = queryable.Where(specification.Criteria);

        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpressions) =>
                current.Include(includeExpressions));

        if (specification.OrderByExpression is not null)
            queryable = queryable.OrderBy(specification.OrderByExpression);
        else if (specification.OrderByDescendingExpression is not null)
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);

        if (specification.IsSplitQuery)
            queryable = queryable.AsSplitQuery();

        if (specification.IsNoTrackingQuery)
            queryable = queryable.AsNoTracking();

        return queryable;
    }
}