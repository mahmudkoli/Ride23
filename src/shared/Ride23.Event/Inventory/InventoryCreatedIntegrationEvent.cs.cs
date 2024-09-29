using Ride23.Framework.Core.Events;

namespace Ride23.Event.Inventory;
public class InventoryCreatedIntegrationEvent : IntegrationEvent
{
    public Guid SupplierId { get; }
    public Guid OrderId { get; }
    public decimal Amount { get; }
    public string Status { get; }

    public InventoryCreatedIntegrationEvent(Guid supplierId, Guid orderId, decimal amount, string status)
    {
        SupplierId = supplierId;
        OrderId = orderId;
        Amount = amount;
        Status = status;
    }
}