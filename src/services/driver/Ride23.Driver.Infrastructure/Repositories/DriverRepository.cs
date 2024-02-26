using Ride23.Driver.Application.Drivers;
using Ride23.Driver.Application.Drivers.Dtos;
using Cust = Ride23.Driver.Domain.Drivers;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Core.Services;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Driver.Infrastructure.Repositories;

public class DriverRepository : EFCoreRepository<Cust.Driver, Guid>, IDriverRepository
{
    private readonly IEFCoreDbContext _dbContext;
    public DriverRepository(IEFCoreDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<DriverDto>> GetPagedDriversAsync<DriverDto>(DriversParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.DbSet<Cust.Driver>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.Name.ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Cust.Driver, DriverDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
