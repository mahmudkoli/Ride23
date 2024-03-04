using Ride23.Framework.Core.Exceptions;

namespace Ride23.Order.Application.Orders.Exceptions;
internal class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(object orderId) : base($"Order with ID '{orderId}' is not found.")
    {
    }
}
