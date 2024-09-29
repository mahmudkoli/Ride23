namespace Ride23.Driver.Application.Common;

public interface IUserService
{
    Task<string> CreateUserAsync(string name, string userName, string email, string password, string phoneNumber);
}
