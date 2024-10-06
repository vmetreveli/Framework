using System.Linq.Expressions;
using Framework.Abstractions.Primitives;

namespace Framework.Abstractions.Specifications;

/// <summary>
///     Represents the abstract base class for specifications, providing criteria and options for querying entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being specified.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class Specification<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : notnull
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Specification{TEntity, TId}" /> class with optional criteria.
    /// </summary>
    /// <param name="criteria">An optional expression to filter entities based on a condition.</param>
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    ///     Gets the criteria used to filter entities.
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    ///     Gets or sets a value indicating whether the query should use split queries.
    /// </summary>
    public bool IsSplitQuery { get; protected set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the query should use no-tracking.
    /// </summary>
    public bool IsNoTrackingQuery { get; protected set; }

    /// <summary>
    ///     Gets the list of expressions used to include related entities.
    /// </summary>
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    /// <summary>
    ///     Gets the expression used to order entities in ascending order.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    /// <summary>
    ///     Gets the expression used to order entities in descending order.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    /// <summary>
    ///     Adds an include expression to the specification for eager loading of related entities.
    /// </summary>
    /// <param name="includeExpression">The include expression to add.</param>
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }

    /// <summary>
    ///     Sets the expression for ordering entities in ascending order.
    /// </summary>
    /// <param name="orderByExpression">The ordering expression to set.</param>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    /// <summary>
    ///     Sets the expression for ordering entities in descending order.
    /// </summary>
    /// <param name="orderByDescendingExpression">The ordering expression to set.</param>
    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }
}