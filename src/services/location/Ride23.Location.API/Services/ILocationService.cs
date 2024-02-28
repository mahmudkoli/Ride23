using Ride23.Location.API.Dtos;

namespace Ride23.Location.API.Services;

public interface ILocationService
{
    Task<IList<LocationDto>> GetLocationsAsync(string identityId);
    Task<LocationDto> UpdateLocationAsync(AddLocationDto locationDto);
}
