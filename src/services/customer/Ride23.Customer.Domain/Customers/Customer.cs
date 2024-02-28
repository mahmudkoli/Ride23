using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Domain;

namespace Ride23.Customer.Domain.Customers;

public class Customer : AuditableEntity
{
    public string IdentityId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsActive { get; set; } = true;

    public Customer Update(
        string name)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        return this;
    }

    public static Customer Create(
        string identityId,
        string name)
    {
        Customer customer = new()
        {
            IdentityId = identityId,
            Name = name!,
            IsActive = true
        };

        var @event = new CustomerCreatedDomainEvent(customer.IdentityId, customer.Id, customer.Name);
        customer.AddDomainEvent(@event);

        return customer;
    }
}
