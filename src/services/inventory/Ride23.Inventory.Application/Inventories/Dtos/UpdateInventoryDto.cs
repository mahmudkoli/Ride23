using Ride23.Inventory.Domain.Inventories.Enums;

namespace Ride23.Inventory.Application.Inventories.Dtos;
public sealed class UpdateInventoryDto
{
    public InventoryStatus Status { get; init; } = default!;
}