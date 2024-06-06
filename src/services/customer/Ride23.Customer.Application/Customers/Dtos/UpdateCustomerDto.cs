namespace Ride23.Customer.Application.Customers.Dtos;
public sealed class UpdateCustomerDto
{
    public string Name { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string ProfilePhoto { get; init; } = default!;
}