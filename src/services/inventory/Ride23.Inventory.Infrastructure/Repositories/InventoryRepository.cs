using Ride23.Inventory.Application.Inventories;
using Ride23.Inventory.Application.Inventories.Dtos;
using Cust = Ride23.Inventory.Domain.Inventories;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Core.Services;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Inventory.Infrastructure.Repositories;

public class InventoryRepository : EFCoreRepository<Cust.Inventory, Guid>, IInventoryRepository
{
    private readonly IEFCoreDbContext _dbContext;
    public InventoryRepository(IEFCoreDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<InventoryDto>> GetPagedInventoriesAsync<InventoryDto>(InventoriesParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.DbSet<Cust.Inventory>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.SupplierId.ToString().ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Cust.Inventory, InventoryDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
