using Ride23.Framework.Core.Events;

namespace Ride23.Event.Customer;
public class CustomerCreatedIntegrationEvent : IntegrationEvent
{
    public string IdentityId { get; }
    public Guid CustomerId { get; }
    public string CustomerName { get; }

    public CustomerCreatedIntegrationEvent(string identityId, Guid customerId, string customerName)
    {
        IdentityId = identityId;
        CustomerId = customerId;
        CustomerName = customerName;
    }
}
