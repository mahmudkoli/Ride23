using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Domain;

namespace Ride23.Customer.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public Guid IdentityGuid { get; private set; }
        public string Name { get; private set; } = default!;
        public bool IsActive { get; set; } = true;

        public Customer Update(
            string name)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            return this;
        }

        public static Customer Create(
            Guid identityGuid,
            string name)
        {
            Customer customer = new()
            {
                IdentityGuid = identityGuid,
                Name = name!,
                IsActive = true
            };

            var @event = new CustomerCreatedDomainEvent(customer.IdentityGuid, customer.Id, customer.Name);
            customer.AddDomainEvent(@event);

            return customer;
        }
    }
}
