using Ride23.Order.Application.Orders.Dtos;
using Cust = Ride23.Order.Domain.Orders;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Pagination;

namespace Ride23.Order.Application.Orders;
public interface IOrderRepository : IRepository<Cust.Order, Guid>
{
    Task<PagedList<OrderDto>> GetPagedOrdersAsync<OrderDto>(OrdersParametersDto parameters, CancellationToken cancellationToken = default);
}