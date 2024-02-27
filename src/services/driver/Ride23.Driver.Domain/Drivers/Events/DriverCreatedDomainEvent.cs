using Ride23.Framework.Core.Events;

namespace Ride23.Driver.Domain.Drivers.Events
{
    public class DriverCreatedDomainEvent : DomainEvent
    {
        public Guid IdentityGuid { get; }
        public Guid DriverId { get; }
        public string DriverName { get; }

        public DriverCreatedDomainEvent(Guid identityGuid, Guid driverId, string driverName)
        {
            IdentityGuid = identityGuid;
            DriverId = driverId;
            DriverName = driverName;
        }
    }
}
