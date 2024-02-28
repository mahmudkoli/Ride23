namespace Ride23.Framework.Core.Services;

public interface ICurrentUserService : IScopedService
{
    string? UserId();
    bool IsAuthenticated();
}