using System.Linq.Expressions;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Specifications;

namespace Framework.Abstractions.Repository;

public interface IRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TEntity> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
    IAsyncEnumerable<TEntity> FindAsync(Specification<TEntity, TId> specification);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}