using Ride23.Framework.Core.Events;
using Ride23.Inventory.Domain.Inventories.Enums;

namespace Ride23.Inventory.Domain.Inventories.Events
{
    public class InventoryCreatedDomainEvent : DomainEvent
    {
        public Guid SupplierId { get; }
        public Guid InventoryId { get; }
        public decimal Amount { get; }
        public InventoryStatus Status { get; }

        public InventoryCreatedDomainEvent(Guid supplierId, Guid inventoryId, decimal amount, InventoryStatus status)
        {
            SupplierId = supplierId;
            InventoryId = inventoryId;
            Amount = amount;
            Status = status;
        }
    }
}
