using Ride23.Framework.Core.Events;

namespace Ride23.Event.Location;
public class DriverLocationCreatedIntegrationEvent : IntegrationEvent
{
    public string IdentityId { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string CellIndex { get; private set; } = default!;
    public DateTime Timestamp { get; private set; }

    public DriverLocationCreatedIntegrationEvent(
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
