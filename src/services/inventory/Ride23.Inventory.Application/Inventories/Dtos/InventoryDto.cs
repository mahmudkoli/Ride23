using Ride23.Inventory.Domain.Inventories.Enums;

namespace Ride23.Inventory.Application.Inventories.Dtos;
public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public decimal Amount { get; set; } = default!;
    public InventoryStatus Status{ get; set; } = default!;
}
