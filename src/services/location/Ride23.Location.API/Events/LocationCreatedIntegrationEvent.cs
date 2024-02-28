using Ride23.Framework.Core.Events;

namespace Ride23.Location.API.Events;
public class LocationCreatedIntegrationEvent : IntegrationEvent
{
    public string IdentityId { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public LocationCreatedIntegrationEvent(string identityId, double latitude, double longitude)
    {
        IdentityId = identityId;
        Latitude = latitude;
        Latitude = longitude;
    }
}
