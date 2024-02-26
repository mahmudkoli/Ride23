using Ride23.Framework.Core.Events;

namespace Ride23.Driver.Domain.Drivers.Events
{
    public class DriverCreatedDomainEvent : DomainEvent
    {
        public Guid IdentityGuid { get; }
        public Guid CustomerId { get; }
        public string CustomerName { get; }

        public DriverCreatedDomainEvent(Guid identityGuid, Guid customerId, string customerName)
        {
            IdentityGuid = identityGuid;
            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}
