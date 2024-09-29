using Ride23.Framework.Core.Domain;
using Ride23.Location.API.Events;

namespace Ride23.Location.API.Entities;

public class DriverLocation : OnlyCreatableEntity
{
    public string IdentityId { get; private set; } = default!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string CellIndex { get; private set; } = default!;
    public DateTime Timestamp { get; private set; }

    public static DriverLocation Create(
        string identityId,
        double latitude,
        double longitude,
        string cellIndex)
    {
        DriverLocation location = new()
        {
            IdentityId = identityId,
            Latitude = latitude,
            Longitude = longitude,
            CellIndex = cellIndex,
            Timestamp = DateTime.UtcNow
        };

        var @event = new DriverLocationCreatedDomainEvent(
            location.IdentityId,
            location.Latitude,
            location.Longitude,
            location.CellIndex,
            location.Timestamp);
        location.AddDomainEvent(@event);

        return location;
    }
}
