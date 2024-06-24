using Ride23.Driver.Application.Common;
using UserGrpc;

namespace Ride23.Driver.Infrastructure.Service
{
    internal class UserService : IUserService
    {
        private readonly User.UserClient _userClient;

        public UserService(User.UserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<string> CreateUserAsync(string name, string userName, string email, string passwordHash, string phoneNumber)
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = name,
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                PhoneNumber = phoneNumber
            };

            var createUserResponse = await _userClient.CreateUserAsync(createUserRequest);
            return createUserResponse.Id;
        }
    }
}
