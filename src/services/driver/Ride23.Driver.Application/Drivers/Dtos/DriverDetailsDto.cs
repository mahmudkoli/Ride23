namespace Ride23.Driver.Application.Drivers.Dtos;
public class DriverDetailsDto
{
    public Guid IdentityId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; } = null;
}
