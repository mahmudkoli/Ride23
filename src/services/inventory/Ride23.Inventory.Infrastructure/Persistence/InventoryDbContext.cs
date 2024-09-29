using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Inventory.Infrastructure.Persistence;

internal class InventoryDbContext : EFCoreDbContext
{
    public InventoryDbContext(
        DbContextOptions contextOptions,
        IEventPublisher events,
        IOptions<EFCoreOptions> options)
        : base(contextOptions, events, options) { }
}
