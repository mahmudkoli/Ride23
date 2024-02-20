using Microsoft.EntityFrameworkCore;

namespace Ride23.Framework.Persistence.EFCore;
public class EFCoreDbContext : DbContext, IEFCoreDbContext
{
    public EFCoreDbContext(DbContextOptions options) : base(options) { }

    public DbSet<T> DbSet<T>() where T : class => Set<T>();

    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => 
        base.SaveChangesAsync(cancellationToken);
}
