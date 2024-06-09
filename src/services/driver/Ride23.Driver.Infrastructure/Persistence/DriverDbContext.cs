using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ride23.Driver.Domain.Drivers.ValueObjects;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Write Fluent API configurations here

        //Driver Property Configurations
        modelBuilder.Entity<Domain.Drivers.Driver>(entity =>
        {
            entity.ToTable("Drivers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IdentityId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(11).IsFixedLength();
            entity.Property(e => e.LicenseNo).IsRequired();
            entity.Property(e => e.LicenseExpiryDate).IsRequired();
            entity.Property(e => e.NoOfRides).IsRequired();
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(100).HasColumnName(nameof(Address.Street));
                address.Property(a => a.City).HasMaxLength(100).HasColumnName(nameof(Address.City));
                address.Property(a => a.PostalCode).HasMaxLength(100).HasColumnName(nameof(Address.PostalCode));
                address.Property(a => a.Country).HasMaxLength(100).HasColumnName(nameof(Address.Country));
            });
        });
    }
}
