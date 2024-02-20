using Microsoft.EntityFrameworkCore;
using Ride23.Framework.Persistence.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ride23.Customer.Infrastructure.Persistence;

internal class CustomerDbContext : EFCoreDbContext
{
    private const string CustomerSchemaName = "Customer";

    public CustomerDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(CustomerSchemaName);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
