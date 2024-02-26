using Ride23.Driver.Application.Drivers.Dtos;
using Cust = Ride23.Driver.Domain.Drivers;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Pagination;

namespace Ride23.Driver.Application.Drivers;
public interface IDriverRepository : IRepository<Cust.Driver, Guid>
{
    Task<PagedList<DriverDto>> GetPagedDriversAsync<DriverDto>(DriversParametersDto parameters, CancellationToken cancellationToken = default);
}