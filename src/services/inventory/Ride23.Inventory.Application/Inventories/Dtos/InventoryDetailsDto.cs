using Ride23.Inventory.Domain.Inventories.Enums;

namespace Ride23.Inventory.Application.Inventories.Dtos;
public class InventoryDetailsDto
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public decimal Amount { get; set; }
    public InventoryStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
}
