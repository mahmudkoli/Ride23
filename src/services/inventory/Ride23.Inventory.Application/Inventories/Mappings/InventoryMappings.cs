using Ride23.Inventory.Application.Inventories.Dtos;
using Cust = Ride23.Inventory.Domain.Inventories;
using Mapster;

namespace Ride23.Inventory.Application.Inventories.Mappings;
public sealed class InventoryMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cust.Inventory, InventoryDto>();
        config.NewConfig<Cust.Inventory, InventoryDetailsDto>();
    }
}