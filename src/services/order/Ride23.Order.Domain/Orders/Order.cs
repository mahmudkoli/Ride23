using Ride23.Order.Domain.Orders.Events;
using Ride23.Framework.Core.Domain;
using Ride23.Order.Domain.Orders.Enums;

namespace Ride23.Order.Domain.Orders;

public class Order : OnlyCreatableEntity
{
    public Guid CustomerId { get; private set; }
    public decimal Amount { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = default!;

    public Order Update(
        OrderStatus status)
    {
        Status = status;
        return this;
    }

    public static Order Create(
        Guid customerId,
        decimal amount)
    {
        Order order = new()
        {
            CustomerId = customerId,
            Amount = amount,
            Status = OrderStatus.Pending
        };

        var @event = new OrderCreatedDomainEvent(order.CustomerId, order.Id, order.Amount, order.Status);
        order.AddDomainEvent(@event);

        return order;
    }
}
