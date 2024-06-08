using Ride23.Driver.Domain.Drivers.Events;
using Ride23.Driver.Domain.Drivers.ValueObjects;
using Ride23.Framework.Core.Domain;

namespace Ride23.Driver.Domain.Drivers;

public class Driver : AuditableEntity
{
    public string IdentityId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public Address Address { get; init; } = default!;
    public string LicenseNo { get; set; } = default!;
    public DateTime LicenseExpiryDate { get; set; }
    public int Status { get; set; } = 1;
    public int NoOfRides { get; set; }
    public string? ProfilePhoto { get; set; }
    public bool IsActive { get; set; } = true;

    public Driver Update(
        string name)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        return this;
    }

    public static Driver Create(
        string identityId,
        string name,
        string phoneNumber,
        Address address,
        string licenseNo,
        DateTime licenseExpiryDate,
        int noOfRides,
        string? profilePhoto)
    {
        Driver driver = new()
        {
            IdentityId = identityId,
            Name = name,
            PhoneNumber = phoneNumber,
            Address = address,
            LicenseNo = licenseNo,
            LicenseExpiryDate = licenseExpiryDate,
            NoOfRides = noOfRides,
            ProfilePhoto = profilePhoto
        };

        var @event = new DriverCreatedDomainEvent(driver.IdentityId, driver.Id, driver.Name);
        driver.AddDomainEvent(@event);

        return driver;
    }
}
