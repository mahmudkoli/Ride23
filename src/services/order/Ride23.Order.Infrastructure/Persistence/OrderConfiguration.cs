using Cust = Ride23.Order.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ride23.Order.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<Cust.Order>
{
    public void Configure(EntityTypeBuilder<Cust.Order> builder)
    {
        const string OrderSchemaName = "Order";
        builder.ToTable("Orders", OrderSchemaName);
    }
}
