using Ride23.Driver.Application.Common;
using Ride23.Framework.Core.gRPC;
using Ride23.Grpc.UserGrpc;

namespace Ride23.Driver.Infrastructure.gRPC;

internal class UserService : IUserService
{
    private readonly IGrpcClient<User.UserClient> _userClient;

    public UserService(
        IGrpcClient<User.UserClient> userClient)
    {
        _userClient = userClient;
    }

    public async Task<string> CreateUserAsync(
        string name,
        string userName,
        string email,
        string password,
        string phoneNumber)
    {
        var createUserRequest = new CreateUserRequest
        {
            Name = name,
            UserName = userName,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber
        };

        var createUserResponse = await _userClient.CallAsync(async client => await client.CreateUserAsync(createUserRequest), createUserRequest);
        return createUserResponse.Id;
    }
}
