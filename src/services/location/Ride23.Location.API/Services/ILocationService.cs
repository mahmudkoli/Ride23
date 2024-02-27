using Ride23.Location.API.Dtos;

namespace Ride23.Location.API.Services;

public interface ILocationService
{
    Task<IList<LocationDto>> GetLocationsAsync(Guid identityGuid);
    Task<LocationDto> UpdateLocationAsync(Guid identityGuid, AddLocationDto locationDto);
}
