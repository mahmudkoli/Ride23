using Cust = Ride23.Customer.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ride23.Customer.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<Cust.Customer>
{
    public void Configure(EntityTypeBuilder<Cust.Customer> builder)
    {
        const string CustomerSchemaName = "Customer";
        builder.ToTable("Customers", CustomerSchemaName);
    }
}
