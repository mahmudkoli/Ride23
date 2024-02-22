using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Domain;
using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Persistence.EFCore;
public class EFCoreDbContext : DbContext, IEFCoreDbContext
{
    private readonly IEventPublisher _events;
    private readonly EFCoreOptions _options;

    public EFCoreDbContext(
        DbContextOptions contextOptions,
        IEventPublisher events,
        IOptions<EFCoreOptions> options) : base(contextOptions)
    {
        _events = events;
        _options = options.Value;
    }

    public DbSet<T> DbSet<T>() where T : class => Set<T>();

    public DatabaseFacade Db() => Database;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_options.DefaultSchema);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await SendDomainEventsAsync();

        return result;
    }

    private async Task SendDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker.Entries<IBaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                await _events.PublishAsync(domainEvent);
            }
        }
    }
}
