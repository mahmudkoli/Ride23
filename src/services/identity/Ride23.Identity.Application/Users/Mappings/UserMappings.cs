using Ride23.Identity.Application.Users.Dtos;
using Ride23.Identity.Domain.Users;
using Mapster;

namespace Ride23.Identity.Application.Users.Mappings;
public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, UserDto>();
    }
}
