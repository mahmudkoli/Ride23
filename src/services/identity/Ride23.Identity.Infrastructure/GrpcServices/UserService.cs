using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Ride23.Identity.Domain.Users;
using UserGrpc;

namespace Ride23.Identity.Infrastructure.GrpcServices
{
    internal class UserService : User.UserBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var user = new AppUser
            {
                Name = request.Name,
                UserName = request.Email,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                PhoneNumber = request.PhoneNumber
            };

            await _userManager.CreateAsync(user, user.PasswordHash);

            return new CreateUserResponse()
            {
                Id = user.Id
            };
        }
    }
}
