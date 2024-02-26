using Ride23.Framework.Core.Events;

namespace Ride23.Driver.Domain.Drivers.Events
{
    public class DriverCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid IdentityGuid { get; }
        public Guid CustomerId { get; }
        public string CustomerName { get; }

        public DriverCreatedIntegrationEvent(Guid identityGuid, Guid customerId, string customerName)
        {
            IdentityGuid = identityGuid;
            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}
