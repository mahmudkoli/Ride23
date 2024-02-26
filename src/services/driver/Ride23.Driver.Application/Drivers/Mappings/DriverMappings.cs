using Ride23.Driver.Application.Drivers.Dtos;
using Cust = Ride23.Driver.Domain.Drivers;
using Mapster;

namespace Ride23.Driver.Application.Drivers.Mappings;
public sealed class DriverMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cust.Driver, DriverDto>();
        config.NewConfig<Cust.Driver, DriverDetailsDto>();
    }
}