using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Domain;

namespace Ride23.Customer.Domain.Customers
{
    public class Customer : AuditableEntity
    {
        public string IdentityId { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public bool IsActive { get; set; } = true;

        public Address Address { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public string ProfilePhoto { get; private set; } = default!;

        public Customer Update(
            string name,
            Address address,
            string phoneNumber,
            string profilePhoto)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (address is not null && !Address.Equals(address)) Address = address;
            if (phoneNumber is not null && PhoneNumber?.Equals(phoneNumber) is not true) PhoneNumber = phoneNumber;
            if (profilePhoto is not null && ProfilePhoto?.Equals(profilePhoto) is not true) ProfilePhoto = profilePhoto;

            return this;
        }

        public static Customer Create(
            string identityId,
            string name,
            Address address,
            string phoneNumber,
            string profilePhoto)
        {
            Customer customer = new()
            {
                IdentityId = identityId,
                Name = name!,
                Address = address!,
                PhoneNumber = phoneNumber!,
                ProfilePhoto = profilePhoto!,
                IsActive = true
            };

            var @event = new CustomerCreatedDomainEvent(customer.IdentityId, customer.Id, customer.Name);
            customer.AddDomainEvent(@event);

            return customer;
        }
    }

    public record Address(string Street, string City, string PostalCode, string Country);
}
