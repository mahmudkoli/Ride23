using Ride23.Order.Domain.Orders.Enums;

namespace Ride23.Order.Application.Orders.Dtos;
public class OrderDetailsDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
}
