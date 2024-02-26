using Ride23.Framework.Core.Pagination;

namespace Ride23.Driver.Application.Drivers.Dtos;
public class DriversParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
