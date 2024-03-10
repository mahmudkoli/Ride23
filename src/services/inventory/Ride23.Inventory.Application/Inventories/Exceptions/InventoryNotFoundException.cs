using Ride23.Framework.Core.Exceptions;

namespace Ride23.Inventory.Application.Inventories.Exceptions;
internal class InventoryNotFoundException : NotFoundException
{
    public InventoryNotFoundException(object inventoryId) : base($"Inventory with ID '{inventoryId}' is not found.")
    {
    }
}
