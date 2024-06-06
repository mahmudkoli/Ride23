namespace Ride23.Customer.Application.Customers.Dtos
{
    public sealed class AddressDto
    {
        public string Street { get; init; } = default!;
        public string City { get; init; } = default!;
        public string PostalCode { get; init; } = default!;
        public string Country { get; init; } = default!;
    }
}
