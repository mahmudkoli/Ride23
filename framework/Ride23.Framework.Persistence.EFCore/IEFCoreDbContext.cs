using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ride23.Framework.Persistence.EFCore;

public interface IEFCoreDbContext : IDisposable
{
    DbSet<T> DbSet<T>() where T : class;
    DatabaseFacade Db();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
