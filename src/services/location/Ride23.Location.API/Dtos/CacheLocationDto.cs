using Ride23.Location.API.Entities;

namespace Ride23.Location.API.Dtos;

public class CacheLocationDto
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CellIndex { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    
    public CacheLocationDto()
    {
        
    }

    public CacheLocationDto(DriverLocation driverLocation)
    {
        this.Id = driverLocation.Id;
        this.IdentityId = driverLocation.IdentityId;
        this.Latitude = driverLocation.Latitude;
        this.Longitude = driverLocation.Longitude;
        this.CellIndex = driverLocation.CellIndex;
        this.Timestamp = driverLocation.Timestamp;
    }
}
