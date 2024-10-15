using System.Linq.Expressions;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Repository;
using Framework.Abstractions.Specifications;

// Provides functionality to work with expressions
// Contains the AggregateRoot base class
// Contains repository interface definitions

// Provides specifications for querying

namespace Framework.Infrastructure.Repository;

/// <summary>
///     Abstract repository implementation for entity operations using Entity Framework.
///     This class implements <see cref="IRepository{TEntity,TId}" /> and provides basic CRUD operations
///     for entities with a specified primary key type.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context used by this repository.</typeparam>
/// <typeparam name="TEntity">The type of the entity this repository works with.</typeparam>
/// <typeparam name="TId">The type of the entity's primary key.</typeparam>
public abstract class RepositoryBase<TDbContext, TEntity, TId>(TDbContext context) : IRepositoryBase<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    ///     Asynchronously gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .FindAsync([id], cancellationToken);
    }

    /// <summary>
    ///     Asynchronously retrieves the first entity matching the given predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The first entity that matches the predicate; otherwise, null.</returns>
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously retrieves the first entity that satisfies the specification.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The first entity that satisfies the specification; otherwise, null.</returns>
    public async Task<TEntity?> FirstOrDefaultAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously retrieves all entities from the database.
    /// </summary>
    /// <returns>An asynchronous stream of entities.</returns>
    public virtual IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return context
            .Set<TEntity>()
            .AsAsyncEnumerable();
    }

    /// <summary>
    ///     Asynchronously retrieves all entities from the database as a list.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of entities.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously finds entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>An asynchronous stream of entities.</returns>
    public virtual IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return context
            .Set<TEntity>()
            .Where(predicate)
            .AsAsyncEnumerable();
    }

    /// <summary>
    ///     Asynchronously finds entities that satisfy the specification.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <returns>An asynchronous stream of entities.</returns>
    public IAsyncEnumerable<TEntity> FindAsync(Specification<TEntity, TId> specification)
    {
        return ApplySpecification(specification)
            .AsAsyncEnumerable();
    }

    /// <summary>
    ///     Asynchronously finds entities that match the specified predicate and returns them as a list.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of entities.</returns>
    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously finds entities that satisfy the specification and returns them as a list.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of entities.</returns>
    public async Task<IEnumerable<TEntity>> FindAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await context
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously adds a range of entities to the database.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await context
            .Set<TEntity>()
            .AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously checks if an entity with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .AnyAsync(
                entity => Equals(entity.Id, id),
                cancellationToken);
    }

    /// <summary>
    ///     Asynchronously checks if any entities match the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>True if any entities match the predicate; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .AnyAsync(predicate, cancellationToken);
    }

    /// <summary>
    ///     Removes an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public void Remove(TEntity entity)
    {
        context
            .Set<TEntity>()
            .Remove(entity);
    }

    /// <summary>
    ///     Removes a range of entities from the database.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context
            .Set<TEntity>()
            .RemoveRange(entities);
    }

    /// <summary>
    ///     Applies the given specification to the entity query.
    /// </summary>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>An IQueryable representing the filtered entity set.</returns>
    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity, TId> specification)
    {
        return SpecificationEvaluator
            .GetQuery(
                context.Set<TEntity>(),
                specification);
    }
}