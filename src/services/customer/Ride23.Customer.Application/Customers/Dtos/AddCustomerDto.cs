namespace Ride23.Customer.Application.Customers.Dtos;
public sealed class AddCustomerDto
{
    public string Name { get; set; } = default!;
    public AddressDto Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string ProfilePhoto { get; init; } = default!;
    public string Password { get; init; } = default!;
}