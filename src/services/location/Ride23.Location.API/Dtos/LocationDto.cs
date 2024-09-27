namespace Ride23.Location.API.Dtos;
public class LocationDto
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CellIndex { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
