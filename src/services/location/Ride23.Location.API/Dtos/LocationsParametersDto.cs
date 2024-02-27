using Ride23.Framework.Core.Pagination;

namespace Ride23.Location.API.Dtos;
public class LocationsParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
