using Microsoft.AspNetCore.Http;
using Ride23.Framework.Core.Services;
using System.Security.Claims;

namespace Ride23.Framework.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private ClaimsPrincipal? _user;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor?.HttpContext?.User;
    }

    public string? UserId() =>
        _user?.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;
}