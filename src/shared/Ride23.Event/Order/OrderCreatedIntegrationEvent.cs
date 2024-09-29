using Ride23.Framework.Core.Events;

namespace Ride23.Event.Order;
public class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public Guid CustomerId { get; }
    public Guid OrderId { get; }
    public decimal Amount { get; }
    public string Status { get; }

    public OrderCreatedIntegrationEvent(Guid customerId, Guid orderId, decimal amount, string status)
    {
        CustomerId = customerId;
        OrderId = orderId;
        Amount = amount;
        Status = status;
    }
}