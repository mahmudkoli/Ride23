using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Domain;
using Ride23.Framework.Core.Events;
using System.Linq.Expressions;

namespace Ride23.Framework.Persistence.EFCore;

/// <summary>
/// The repository that implements IRepositoryWithEvents.
/// Implemented as a decorator. It only augments the Add,
/// Update and Delete calls where it adds the respective
/// EntityCreated, EntityUpdated or EntityDeleted event
/// before delegating to the decorated repository.
/// </summary>
public class EFCoreEventAddingRepositoryDecorator<TEntity, TId> : IRepositoryWithEvents<TEntity, TId>
    where TEntity : class, IBaseEntity<TId>
{
    private readonly IRepository<TEntity, TId> _decorated;

    public EFCoreEventAddingRepositoryDecorator(IRepository<TEntity, TId> decorated) => _decorated = decorated;


    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _decorated.ExistsAsync(predicate, cancellationToken: cancellationToken)!;
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _decorated.FindAsync(predicate, cancellationToken: cancellationToken)!;
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _decorated.FindOneAsync(predicate, cancellationToken: cancellationToken)!;
    }

    public Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return _decorated.FindByIdAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _decorated.GetAllAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.AddDomainEvent(EntityCreatedEvent.WithEntity(entity));
        await _decorated.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.AddDomainEvent(EntityUpdatedEvent.WithEntity(entity));
        await _decorated.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.AddDomainEvent(EntityDeletedEvent.WithEntity(entity));
        }
        await _decorated.DeleteRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = await FindAsync(predicate, cancellationToken);
        foreach (var entity in entities)
        {
            entity.AddDomainEvent(EntityDeletedEvent.WithEntity(entity));
        }
        await _decorated.DeleteRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.AddDomainEvent(EntityDeletedEvent.WithEntity(entity));
        await _decorated.DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        entity.AddDomainEvent(EntityDeletedEvent.WithEntity(entity));
        await _decorated.DeleteAsync(entity, cancellationToken);
    }

    public void Dispose()
    {
        _decorated.Dispose();
    }
}