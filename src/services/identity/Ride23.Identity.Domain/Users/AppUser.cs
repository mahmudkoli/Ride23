using Microsoft.AspNetCore.Identity;

namespace Ride23.Identity.Domain.Users;
public class AppUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}