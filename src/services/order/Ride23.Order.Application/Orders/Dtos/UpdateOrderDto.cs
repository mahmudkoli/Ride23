using Ride23.Order.Domain.Orders.Enums;

namespace Ride23.Order.Application.Orders.Dtos;
public sealed class UpdateOrderDto
{
    public OrderStatus Status { get; init; } = default!;
}