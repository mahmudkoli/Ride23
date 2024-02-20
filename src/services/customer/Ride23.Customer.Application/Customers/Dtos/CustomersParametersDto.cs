using Ride23.Framework.Core.Pagination;

namespace Ride23.Customer.Application.Customers.Dtos;
public class CustomersParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
