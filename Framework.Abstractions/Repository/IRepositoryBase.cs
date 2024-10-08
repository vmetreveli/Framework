using System.Linq.Expressions;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Specifications;

namespace Framework.Abstractions.Repository;

/// <summary>
///     Defines the contract for a repository that manages the persistence and retrieval of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public interface IRepositoryBase<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    ///     Asynchronously retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the entity or null if not found.</returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the first entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression that defines the criteria to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the entity or null if not found.</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the first entity that matches the specified specification.
    /// </summary>
    /// <param name="specification">A specification defining the criteria to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the entity or null if not found.</returns>
    Task<TEntity?> FirstOrDefaultAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>An asynchronous sequence of all entities.</returns>
    IAsyncEnumerable<TEntity> GetAllAsync();

    /// <summary>
    ///     Asynchronously retrieves all entities.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of a list of all entities.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves entities that match the specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">An expression that defines the criteria to filter entities.</param>
    /// <returns>An asynchronous sequence of entities that match the predicate.</returns>
    IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Retrieves entities that match the specified specification asynchronously.
    /// </summary>
    /// <param name="specification">A specification defining the criteria to filter entities.</param>
    /// <returns>An asynchronous sequence of entities that match the specification.</returns>
    IAsyncEnumerable<TEntity> FindAsync(Specification<TEntity, TId> specification);

    /// <summary>
    ///     Asynchronously retrieves entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression that defines the criteria to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result of a list of entities that match the predicate.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves entities that match the specified specification.
    /// </summary>
    /// <param name="specification">A specification defining the criteria to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, with a result of a list of entities that match the
    ///     specification.
    /// </returns>
    Task<IEnumerable<TEntity>> FindAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously checks if an entity with the specified identifier exists.
    /// </summary>
    /// <param name="id">The identifier of the entity to check.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, with a result indicating whether the entity exists.</returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously checks if an entity that matches the specified predicate exists.
    /// </summary>
    /// <param name="predicate">An expression that defines the criteria to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, with a result indicating whether an entity that matches the
    ///     predicate exists.
    /// </returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously adds a range of entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    ///     Removes a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    void RemoveRange(IEnumerable<TEntity> entities);
}