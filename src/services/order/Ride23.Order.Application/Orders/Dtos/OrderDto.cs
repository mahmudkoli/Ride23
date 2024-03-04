using Ride23.Order.Domain.Orders.Enums;

namespace Ride23.Order.Application.Orders.Dtos;
public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; } = default!;
    public OrderStatus Status{ get; set; } = default!;
}
