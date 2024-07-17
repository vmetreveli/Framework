using System.Linq.Expressions;
using Framework.Abstractions.Primitives;

namespace Framework.Abstractions.Specifications;

/// <summary>
///     Represents the abstract base class for specifications.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class Specification<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull

{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; }
    public bool IsSplitQuery { get; protected set; }
    public bool IsNoTrackingQuery { get; protected set; }
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> ordertByExpression)
    {
        OrderByExpression = ordertByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> ordertByDescendingExpression)
    {
        OrderByDescendingExpression = ordertByDescendingExpression;
    }
}