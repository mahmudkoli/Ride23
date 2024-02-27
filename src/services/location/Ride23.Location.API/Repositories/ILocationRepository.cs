using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Pagination;
using Ride23.Location.API.Dtos;
using Loc = Ride23.Location.API.Entities;

namespace Ride23.Location.API.Repositories;
public interface ILocationRepository : IRepository<Loc.Location, Guid>
{
    Task<PagedList<LocationDto>> GetPagedLocationsAsync<LocationDto>(LocationsParametersDto parameters, CancellationToken cancellationToken = default);
}