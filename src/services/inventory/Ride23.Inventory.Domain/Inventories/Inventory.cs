using Ride23.Inventory.Domain.Inventories.Events;
using Ride23.Framework.Core.Domain;
using Ride23.Inventory.Domain.Inventories.Enums;

namespace Ride23.Inventory.Domain.Inventories;

public class Inventory : OnlyCreatableEntity
{
    public Guid SupplierId { get; private set; }
    public decimal Amount { get; private set; } = default!;
    public InventoryStatus Status { get; private set; } = default!;

    public Inventory Update(
        InventoryStatus status)
    {
        Status = status;
        return this;
    }

    public static Inventory Create(
        Guid supplierId,
        decimal amount)
    {
        Inventory inventory = new()
        {
            SupplierId = supplierId,
            Amount = amount,
            Status = InventoryStatus.Pending
        };

        var @event = new InventoryCreatedDomainEvent(inventory.SupplierId, inventory.Id, inventory.Amount, inventory.Status);
        inventory.AddDomainEvent(@event);

        return inventory;
    }
}
