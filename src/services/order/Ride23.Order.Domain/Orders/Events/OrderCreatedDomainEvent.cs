using Ride23.Framework.Core.Events;
using Ride23.Order.Domain.Orders.Enums;

namespace Ride23.Order.Domain.Orders.Events
{
    public class OrderCreatedDomainEvent : DomainEvent
    {
        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public decimal Amount { get; }
        public OrderStatus Status { get; }

        public OrderCreatedDomainEvent(Guid customerId, Guid orderId, decimal amount, OrderStatus status)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Amount = amount;
            Status = status;
        }
    }
}
