using Ride23.Framework.Core.Pagination;

namespace Ride23.Order.Application.Orders.Dtos;
public class OrdersParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
