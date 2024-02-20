namespace Ride23.Customer.Application.Customers.Dtos;
public class CustomerDetailsDto
{
    public Guid IdentityId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; } = null;
}
