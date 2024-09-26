using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Infrastructure.gRPC;

public class ServiceOptions : IOptionsRoot
{
    public string IdentityServiceUrl { get; set; }
}
