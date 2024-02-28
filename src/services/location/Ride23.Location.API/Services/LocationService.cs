using Ride23.Framework.Core.Caching;
using Ride23.Location.API.Repositories;
using loc = Ride23.Location.API.Entities;
using MapsterMapper;
using Ride23.Location.API.Dtos;
using Ride23.Framework.Core.Services;

namespace Ride23.Location.API.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public LocationService(
        ILocationRepository locationRepository,
        ICacheService cacheService,
        IMapper mapper,
        ICacheKeyService cacheKeyService,
        ICurrentUserService currentUser)
    {
        _locationRepository = locationRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _cacheKeyService = cacheKeyService;
        _currentUser = currentUser;
    }

    public async Task<LocationDto> UpdateLocationAsync(AddLocationDto locationDto)
    {
        var locationToAdd = loc.Location.Create(_currentUser.UserId(), locationDto.Latitude, locationDto.Longitude);
        await _cacheService.SetAsync(
            _cacheKeyService.GetCacheKey<loc.Location>(_currentUser.UserId()), 
            new { locationToAdd.Latitude, locationToAdd.Longitude });
        await _locationRepository.AddAsync(locationToAdd);
        var data = _mapper.Map<LocationDto>(locationToAdd);
        return data;
    }

    public async Task<IList<LocationDto>> GetLocationsAsync(string identityId)
    {
        var result = await _locationRepository.FindAsync(x => x.IdentityId == identityId);
        var data = _mapper.Map<IList<LocationDto>>(result);
        return data;
    }
}
