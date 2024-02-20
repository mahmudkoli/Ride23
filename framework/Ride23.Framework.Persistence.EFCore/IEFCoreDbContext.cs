using Microsoft.EntityFrameworkCore;

namespace Ride23.Framework.Persistence.EFCore;

public interface IEFCoreDbContext : IDisposable
{
    DbSet<T> DbSet<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
