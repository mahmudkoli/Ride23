using Microsoft.EntityFrameworkCore;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Domain;
using Ride23.Framework.Core.Services;
using System.Linq.Expressions;

namespace Ride23.Framework.Persistence.EFCore;
public class EFCoreRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IBaseEntity<TId>
{
    private readonly IEFCoreDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IDateTimeService _dateTimeProvider;

    public EFCoreRepository(IEFCoreDbContext context, IDateTimeService dateTimeProvider)
    {
        _context = context;
        _dbSet = _context.DbSet<TEntity>();
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken: cancellationToken)!;
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken: cancellationToken)!;
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbSet.SingleOrDefaultAsync(predicate, cancellationToken: cancellationToken)!;
    }

    public Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return FindOneAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {

        _dbSet.RemoveRange(await FindAsync(predicate, cancellationToken));
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(await FindByIdAsync(id, cancellationToken));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
