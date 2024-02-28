using Ride23.Framework.Core.Events;

namespace Ride23.Location.API.Events;
public class LocationCreatedDomainEvent : DomainEvent
{
    public string IdentityId { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public LocationCreatedDomainEvent(string identityId, double latitude, double longitude)
    {
        IdentityId = identityId;
        Latitude = latitude;
        Longitude = longitude;
    }
}
