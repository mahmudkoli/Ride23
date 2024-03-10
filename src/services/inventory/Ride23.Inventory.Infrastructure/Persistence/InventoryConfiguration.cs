using Cust = Ride23.Inventory.Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ride23.Inventory.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<Cust.Inventory>
{
    public void Configure(EntityTypeBuilder<Cust.Inventory> builder)
    {
        const string InventorieschemaName = "Inventory";
        builder.ToTable("Inventories", InventorieschemaName);
    }
}
