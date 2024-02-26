using Cust = Ride23.Driver.Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ride23.Driver.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<Cust.Driver>
{
    public void Configure(EntityTypeBuilder<Cust.Driver> builder)
    {
        const string DriverSchemaName = "Driver";
        builder.ToTable("Drivers", DriverSchemaName);
    }
}
