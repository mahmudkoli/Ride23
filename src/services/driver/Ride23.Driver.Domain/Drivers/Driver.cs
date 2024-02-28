using Ride23.Driver.Domain.Drivers.Events;
using Ride23.Framework.Core.Domain;

namespace Ride23.Driver.Domain.Drivers;

public class Driver : AuditableEntity
{
    public string IdentityId { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsActive { get; set; } = true;

    public Driver Update(
        string name)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        return this;
    }

    public static Driver Create(
        string identityId,
        string name)
    {
        Driver customer = new()
        {
            IdentityId = identityId,
            Name = name!,
            IsActive = true
        };

        var @event = new DriverCreatedDomainEvent(customer.IdentityId, customer.Id, customer.Name);
        customer.AddDomainEvent(@event);

        return customer;
    }
}
