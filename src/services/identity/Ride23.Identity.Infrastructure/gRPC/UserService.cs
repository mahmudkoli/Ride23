using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Ride23.Identity.Domain.Users;
using Ride23.Grpc.UserGrpc;

namespace Ride23.Identity.Infrastructure.gRPC;

internal class UserService : User.UserBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<AppUser> userManager, 
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail != null)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Email already registered"));

        var user = new AppUser
        {
            Name = request.Name,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("{error}", error.Description);
            }
            throw new RpcException(new Status(StatusCode.Internal, "Identity exception"));
        }

        return new CreateUserResponse()
        {
            Id = user.Id
        };
    }
}
