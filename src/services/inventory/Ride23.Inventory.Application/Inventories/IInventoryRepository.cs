using Ride23.Inventory.Application.Inventories.Dtos;
using Cust = Ride23.Inventory.Domain.Inventories;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Pagination;

namespace Ride23.Inventory.Application.Inventories;
public interface IInventoryRepository : IRepository<Cust.Inventory, Guid>
{
    Task<PagedList<InventoryDto>> GetPagedInventoriesAsync<InventoryDto>(InventoriesParametersDto parameters, CancellationToken cancellationToken = default);
}