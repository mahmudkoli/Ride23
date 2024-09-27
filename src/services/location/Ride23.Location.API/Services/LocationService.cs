using H3;
using H3.Algorithms;
using H3.Extensions;
using MapsterMapper;
using NetTopologySuite.Geometries;
using Ride23.Framework.Core.Caching;
using Ride23.Framework.Core.Services;
using Ride23.Location.API.Dtos;
using Ride23.Location.API.Repositories;
using Ride23.Location.API.Entities;

namespace Ride23.Location.API.Services;

public class LocationService : ILocationService
{
    private readonly IDriverLocationRepository _driverLocationRepository;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly int _resolution = 9;
    private readonly string _cellIndexCacheKeyPrefix = "CellIndex";

    public LocationService(
        IDriverLocationRepository driverLocationRepository,
        ICacheService cacheService,
        IMapper mapper,
        ICacheKeyService cacheKeyService,
        ICurrentUserService currentUser)
    {
        _driverLocationRepository = driverLocationRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _cacheKeyService = cacheKeyService;
        _currentUser = currentUser;
    }

    public async Task<IList<LocationDto>> GetDriverLocationHistoryAsync(string identityId)
    {
        var result = await _driverLocationRepository.FindAsync(x => x.IdentityId == identityId);
        var data = _mapper.Map<IList<LocationDto>>(result);
        return data;
    }

    public async Task<string> UpdateDriverLocationAsync(AddLocationDto locationDto)
    {
        return await UpdateDriverLocationAsync(_currentUser.UserId(), locationDto.Latitude, locationDto.Longitude);
    }

    public async Task<string> UpdateDriverLocationAsync(
        string identityId,
        double latitude,
        double longitude)
    {
        var coordinate = new Coordinate(longitude, latitude);
        var h3CellIndex = coordinate.ToH3Index(_resolution);
        var cellIndex = h3CellIndex.ToString();

        var existingLocation = await _cacheService.GetAsync<CacheLocationDto>(
            _cacheKeyService.GetCacheKey<DriverLocation>(identityId));
        var newLocation = DriverLocation.Create(identityId, latitude, longitude, cellIndex);

        if (existingLocation != null && existingLocation.CellIndex != newLocation.CellIndex)
            await RemoveDriverFromCellIndexCacheAsync(existingLocation.CellIndex, identityId);
        else
            await AddDriverToCellIndexCacheAsync(cellIndex, identityId);

        await _cacheService.SetAsync(
            _cacheKeyService.GetCacheKey<DriverLocation>(identityId),
            new CacheLocationDto(newLocation));

        await _driverLocationRepository.AddAsync(newLocation);

        return newLocation.CellIndex;
    }

    public async Task<List<CacheLocationDto>> FindNearbyDriversAsync(AddLocationDto locationDto)
    {
        return await FindNearbyDriversAsync(locationDto.Latitude, locationDto.Longitude);
    }

    public async Task<List<CacheLocationDto>> FindNearbyDriversAsync(double latitude, double longitude)
    {
        var coordinate = new Coordinate(longitude, latitude);
        var h3CellIndex = coordinate.ToH3Index(_resolution);

        var neighbors = (h3CellIndex.GridRing(1) ?? Enumerable.Empty<H3Index>())
                            .Append(h3CellIndex);

        var nearbyDrivers = new List<CacheLocationDto>();

        foreach (var neighbor in neighbors)
        {
            var cellIndex = neighbor.ToString();
            var cacheKey = $"{_cellIndexCacheKeyPrefix}-{cellIndex}";
            var identityIds = await _cacheService.GetAsync<HashSet<string>>(cacheKey) ?? new HashSet<string>();

            foreach (var identityId in identityIds)
            {
                var existingLocation = await _cacheService.GetAsync<CacheLocationDto>(
                    _cacheKeyService.GetCacheKey<DriverLocation>(identityId));

                if (existingLocation != null)
                    nearbyDrivers.Add(existingLocation);
            }
        }

        return nearbyDrivers;
    }

    private async Task AddDriverToCellIndexCacheAsync(string cellIndex, string identityId)
    {
        var cacheKey = $"{_cellIndexCacheKeyPrefix}-{cellIndex}";
        var identityIds = await _cacheService.GetAsync<HashSet<string>>(cacheKey) ?? new HashSet<string>();
        identityIds.Add(identityId);
        await _cacheService.SetAsync(cacheKey, identityIds);
    }

    private async Task RemoveDriverFromCellIndexCacheAsync(string cellIndex, string identityId)
    {
        var cacheKey = $"{_cellIndexCacheKeyPrefix}-{cellIndex}";
        var identityIds = await _cacheService.GetAsync<HashSet<string>>(cacheKey);
        if (identityIds != null && identityIds.Any())
        {
            identityIds.Remove(identityId);
            if (identityIds.Any())
                await _cacheService.SetAsync(cacheKey, identityIds);
            else
                await _cacheService.RemoveAsync(cacheKey);
        }
    }
}
