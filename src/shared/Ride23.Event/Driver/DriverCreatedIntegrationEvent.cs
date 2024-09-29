using Ride23.Framework.Core.Events;

namespace Ride23.Event.Driver;
public class DriverCreatedIntegrationEvent : IntegrationEvent
{
    public string IdentityId { get; }
    public Guid DriverId { get; }
    public string DriverName { get; }

    public DriverCreatedIntegrationEvent(string identityId, Guid driverId, string driverName)
    {
        IdentityId = identityId;
        DriverId = driverId;
        DriverName = driverName;
    }
}
