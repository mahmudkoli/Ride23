namespace Ride23.Inventory.Application.Inventories.Dtos;
public sealed class AddInventoryDto
{
    public Guid SupplierId { get; set; }
    public decimal Amount { get; set; }
}