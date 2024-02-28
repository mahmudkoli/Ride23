using Ride23.Framework.Core.Events;

namespace Ride23.Driver.Domain.Drivers.Events
{
    public class DriverCreatedDomainEvent : DomainEvent
    {
        public string IdentityId { get; }
        public Guid DriverId { get; }
        public string DriverName { get; }

        public DriverCreatedDomainEvent(string identityId, Guid driverId, string driverName)
        {
            IdentityId = identityId;
            DriverId = driverId;
            DriverName = driverName;
        }
    }
}
