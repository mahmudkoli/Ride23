using Ride23.Framework.Core.Domain;
using Ride23.Location.API.Events;

namespace Ride23.Location.API.Entities;
public class Location : OnlyCreatableEntity
{
    public Guid IdentityGuid { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public static Location Create(
        Guid identityGuid,
        double latitude,
        double longitude)
    {
        Location location = new()
        {
            IdentityGuid = identityGuid,
            Latitude = latitude,
            Longitude = longitude
        };

        var @event = new LocationCreatedDomainEvent(location.IdentityGuid, location.Latitude, location.Longitude);
        location.AddDomainEvent(@event);

        return location;
    }
}
