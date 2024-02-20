using Ride23.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ride23.Identity.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private const string IdentitySchemaName = "Identity";

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(IdentitySchemaName);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
