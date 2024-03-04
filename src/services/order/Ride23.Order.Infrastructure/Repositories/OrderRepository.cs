using Ride23.Order.Application.Orders;
using Ride23.Order.Application.Orders.Dtos;
using Cust = Ride23.Order.Domain.Orders;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Core.Services;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Order.Infrastructure.Repositories;

public class OrderRepository : EFCoreRepository<Cust.Order, Guid>, IOrderRepository
{
    private readonly IEFCoreDbContext _dbContext;
    public OrderRepository(IEFCoreDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<OrderDto>> GetPagedOrdersAsync<OrderDto>(OrdersParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.DbSet<Cust.Order>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.CustomerId.ToString().ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Cust.Order, OrderDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
