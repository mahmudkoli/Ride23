using Ride23.Framework.Core.Events;

namespace Ride23.Driver.Domain.Drivers.Events
{
    public class DriverCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid IdentityGuid { get; }
        public Guid DriverId { get; }
        public string DriverName { get; }

        public DriverCreatedIntegrationEvent(Guid identityGuid, Guid driverId, string driverName)
        {
            IdentityGuid = identityGuid;
            DriverId = driverId;
            DriverName = driverName;
        }
    }
}
