using Grpc.Core;
using Ride23.Customer.Application.Common;
using Ride23.Customer.Application.Customers.Exceptions;
using System.Net;
using UserGrpc;

namespace Ride23.Customer.Infrastructure.gRPC.Service
{
    public class UserService : IUserService
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

            try
            {
                var createUserResponse = await _userClient.CreateUserAsync(createUserRequest);
                return createUserResponse.Id;
            }
            catch (RpcException ex)
            {
                var errorMessage = $"Failed to create user via gRPC. Status: {ex.Status.StatusCode}, Message: {ex.Status.Detail}";
                throw new GrpcException(errorMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}
