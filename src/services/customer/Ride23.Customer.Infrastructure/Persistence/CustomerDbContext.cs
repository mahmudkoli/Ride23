using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Customer.Infrastructure.Persistence;

internal class CustomerDbContext : EFCoreDbContext
{
    public CustomerDbContext(
        DbContextOptions contextOptions,
        IEventPublisher events,
        IOptions<EFCoreOptions> options)
        : base(contextOptions, events, options) { }
}
