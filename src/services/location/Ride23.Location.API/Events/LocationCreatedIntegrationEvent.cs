using Ride23.Framework.Core.Events;

namespace Ride23.Location.API.Events;
public class LocationCreatedIntegrationEvent : IntegrationEvent
{
    public Guid IdentityGuid { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public LocationCreatedIntegrationEvent(Guid identityGuid, double latitude, double longitude)
    {
        IdentityGuid = identityGuid;
        Latitude = latitude;
        Latitude = longitude;
    }
}
