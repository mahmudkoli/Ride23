using Ride23.Framework.Core.Pagination;

namespace Ride23.Inventory.Application.Inventories.Dtos;
public class InventoriesParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
