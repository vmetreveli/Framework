using Framework.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Framework.Abstractions.Specifications;

/// <summary>
///     Provides methods for evaluating and applying specifications to an <see cref="IQueryable{TEntity}" />.
/// </summary>
public static class SpecificationEvaluator
{
    /// <summary>
    ///     Applies the given <see cref="Specification{TEntity, TId}" /> to the provided <see cref="IQueryable{TEntity}" />.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the query.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    /// <param name="inputQueryable">The initial queryable to which the specification will be applied.</param>
    /// <param name="specification">The specification containing criteria, includes, ordering, and tracking information.</param>
    /// <returns>An <see cref="IQueryable{TEntity}" /> that reflects the applied specification.</returns>
    public static IQueryable<TEntity> GetQuery<TEntity, TId>(
        IQueryable<TEntity> inputQueryable, Specification<TEntity, TId> specification)
        where TEntity : EntityBase<TId>
        where TId : notnull
    {
        var queryable = inputQueryable;

        // Apply the criteria (filter) if it is not null
        if (specification.Criteria is not null)
            queryable = queryable.Where(specification.Criteria);

        // Apply include expressions for eager loading
        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpressions) =>
                current.Include(includeExpressions));

        // Apply ordering if specified
        if (specification.OrderByExpression is not null)
            queryable = queryable.OrderBy(specification.OrderByExpression);
        else if (specification.OrderByDescendingExpression is not null)
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);

        // Apply split query if specified
        if (specification.IsSplitQuery)
            queryable = queryable.AsSplitQuery();

        // Apply no-tracking if specified
        if (specification.IsNoTrackingQuery)
            queryable = queryable.AsNoTracking();

        return queryable;
    }
}