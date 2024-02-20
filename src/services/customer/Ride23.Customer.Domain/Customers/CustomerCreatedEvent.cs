using Ride23.Framework.Core.Events;

namespace Ride23.Customer.Domain.Customers
{
    public class CustomerCreatedEvent : DomainEvent
    {
        public Guid IdentityGuid { get; }
        public Guid CustomerId { get; }
        public string CustomerName { get; }

        public CustomerCreatedEvent(Guid identityGuid, Guid customerId, string customerName)
        {
            IdentityGuid = identityGuid;
            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}
