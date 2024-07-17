using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Framework.Abstractions.Primitives;
using Framework.Abstractions.Repository;
using Framework.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Framework.Infrastructure.Repository;

public abstract class Repository<TDbContext, TEntity, TId>(TDbContext context) : IRepository<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .FindAsync([id], cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public virtual IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return context
            .Set<TEntity>()
            .AsAsyncEnumerable();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public virtual IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return context
            .Set<TEntity>()
            .Where(predicate)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<TEntity> FindAsync(Specification<TEntity, TId> specification)
    {
        return ApplySpecification(specification)
            .AsAsyncEnumerable();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Specification<TEntity, TId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification)
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await context
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await context
            .Set<TEntity>()
            .AddRangeAsync(entities, cancellationToken);
    }


    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .AnyAsync(
                entity => Equals(entity.Id, id),
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Set<TEntity>()
            .AnyAsync(predicate, cancellationToken);
    }


    public void Remove(TEntity entity)
    {
        context
            .Set<TEntity>()
            .Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context
            .Set<TEntity>()
            .RemoveRange(entities);
    }


    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity, TId> specification)
    {
        return SpecificationEvaluator
            .GetQuery(
                context.Set<TEntity>(),
                specification);
    }
}