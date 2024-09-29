using Ride23.Driver.Domain.Drivers.ValueObjects;

namespace Ride23.Driver.Application.Drivers.Dtos
{
    public sealed class RegisterDriverDto
    {
        public string Name { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public Address Address { get; init; } = default!;

        public string LicenseNo { get; set; } = default!;

        public DateTime LicenseExpiryDate { get; set; }

        public int NoOfRides { get; set; }

        public string? ProfilePhoto { get; set; }
    }
}
