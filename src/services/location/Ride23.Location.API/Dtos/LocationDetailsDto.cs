namespace Ride23.Location.API.Dtos;
public class LocationDetailsDto
{
    public Guid IdentityId { get; set; }
    public Guid Id { get; set; }
    public long Latitude { get; set; }
    public long Longitude { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; } = null;
}
