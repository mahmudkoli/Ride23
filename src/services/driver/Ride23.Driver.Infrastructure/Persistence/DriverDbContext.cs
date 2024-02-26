using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Driver.Infrastructure.Persistence;

internal class DriverDbContext : EFCoreDbContext
{
    public DriverDbContext(
        DbContextOptions contextOptions,
        IEventPublisher events,
        IOptions<EFCoreOptions> options)
        : base(contextOptions, events, options) { }
}
