using Ride23.Framework.Core.Exceptions;

namespace Ride23.Driver.Application.Drivers.Exceptions;
internal class DriverNotFoundException : NotFoundException
{
    public DriverNotFoundException(object driverId) : base($"Driver with ID '{driverId}' is not found.")
    {
    }
}
