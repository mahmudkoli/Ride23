using Ride23.Framework.Core.Events;

namespace Ride23.Location.API.Events;
public class DriverLocationCreatedDomainEvent : DomainEvent
{
    public string IdentityId { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string CellIndex { get; private set; }
    public DateTime Timestamp { get; private set; }

    public DriverLocationCreatedDomainEvent(
        string identityId, 
        double latitude, 
        double longitude,
        string cellIndex,
        DateTime timestamp)
    {
        IdentityId = identityId;
        Latitude = latitude;
        Longitude = longitude;
        CellIndex = cellIndex;
        Timestamp = timestamp;
    }
}
