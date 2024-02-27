using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Core.Services;
using Ride23.Framework.Persistence.NoSQL;
using Ride23.Location.API.Dtos;
using Loc = Ride23.Location.API.Entities;

namespace Ride23.Location.API.Repositories;
public class LocationRepository : CassandraRepository<Loc.Location, Guid>, ILocationRepository
{
    private readonly ICassandraDbContext _dbContext;
    public LocationRepository(ICassandraDbContext context, IDateTimeService dateTimeService)
        : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<LocationDto>> GetPagedLocationsAsync<LocationDto>(LocationsParametersDto parameters, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
