using Ride23.Framework.Core.Domain;
using Ride23.Location.API.Events;

namespace Ride23.Location.API.Entities;
public class Location : OnlyCreatableEntity
{
    public string IdentityId { get; private set; } = default!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public static Location Create(
        string identityId,
        double latitude,
        double longitude)
    {
        Location location = new()
        {
            IdentityId = identityId,
            Latitude = latitude,
            Longitude = longitude
        };

        var @event = new LocationCreatedDomainEvent(location.IdentityId, location.Latitude, location.Longitude);
        location.AddDomainEvent(@event);

        return location;
    }
}
