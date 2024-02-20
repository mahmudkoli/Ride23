using Ride23.Customer.Application.Customers.Dtos;
using Cust = Ride23.Customer.Domain.Customers;
using Mapster;

namespace Ride23.Customer.Application.Customers.Mappings;
public sealed class CustomerMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cust.Customer, CustomerDto>();
        config.NewConfig<Cust.Customer, CustomerDetailsDto>();
    }
}