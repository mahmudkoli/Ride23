using Ride23.Location.API.Dtos;

namespace Ride23.Location.API.Services;

public interface ILocationService
{
    Task<IList<LocationDto>> GetDriverLocationHistoryAsync(string identityId);
    Task<string> UpdateDriverLocationAsync(AddLocationDto locationDto);
    Task<string> UpdateDriverLocationAsync(string identityId, double latitude, double longitude);
    Task<List<CacheLocationDto>> FindNearbyDriversAsync(AddLocationDto locationDto);
    Task<List<CacheLocationDto>> FindNearbyDriversAsync(double latitude, double longitude);
}
