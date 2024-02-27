using Ride23.Framework.Core.Caching;
using Ride23.Location.API.Repositories;
using loc = Ride23.Location.API.Entities;
using MapsterMapper;
using Ride23.Location.API.Dtos;

namespace Ride23.Location.API.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyService _cacheKeyService;
    private readonly IMapper _mapper;

    public LocationService(
        ILocationRepository locationRepository,
        ICacheService cacheService,
        IMapper mapper,
        ICacheKeyService cacheKeyService)
    {
        _locationRepository = locationRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _cacheKeyService = cacheKeyService;
    }

    public async Task<LocationDto> UpdateLocationAsync(Guid identityGuid, AddLocationDto locationDto)
    {
        var locationToAdd = loc.Location.Create(identityGuid, locationDto.Latitude, locationDto.Longitude);
        await _cacheService.SetAsync(
            _cacheKeyService.GetCacheKey<loc.Location>(identityGuid), 
            new { locationToAdd.Latitude, locationToAdd.Longitude });
        await _locationRepository.AddAsync(locationToAdd);
        var data = _mapper.Map<LocationDto>(locationToAdd);
        return data;
    }

    public async Task<IList<LocationDto>> GetLocationsAsync(Guid identityGuid)
    {
        var result = await _locationRepository.FindAsync(x => x.IdentityGuid == identityGuid);
        var data = _mapper.Map<IList<LocationDto>>(result);
        return data;
    }
}
