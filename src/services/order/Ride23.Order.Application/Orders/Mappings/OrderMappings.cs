using Ride23.Order.Application.Orders.Dtos;
using Cust = Ride23.Order.Domain.Orders;
using Mapster;

namespace Ride23.Order.Application.Orders.Mappings;
public sealed class OrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cust.Order, OrderDto>();
        config.NewConfig<Cust.Order, OrderDetailsDto>();
    }
}