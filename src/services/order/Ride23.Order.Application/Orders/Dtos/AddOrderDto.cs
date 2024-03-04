namespace Ride23.Order.Application.Orders.Dtos;
public sealed class AddOrderDto
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}