using Microsoft.EntityFrameworkCore.Storage;
using MSFA23.Application.Common.Persistence;
using System.Data;

namespace Ride23.Framework.Persistence.EFCore;

public class EFCoreUnitOfWork : IUnitOfWork
{
    private readonly IEFCoreDbContext _context;
    private IDbContextTransaction? _transaction;

    public EFCoreUnitOfWork(IEFCoreDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null) return;

        _transaction = await _context.Db().BeginTransactionAsync(cancellationToken);
    }

    public IDbTransaction? GetDbTransaction()
    {
        return _transaction?.GetDbTransaction();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;

        await _transaction.CommitAsync(cancellationToken);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task SaveAndCommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        await CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync(cancellationToken);
        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_transaction == null) return;

        _transaction.Dispose();
        _transaction = null;
    }
}