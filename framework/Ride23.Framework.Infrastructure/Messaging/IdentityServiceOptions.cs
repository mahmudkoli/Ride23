using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Infrastructure.Messaging
{
    public class IdentityServiceOptions : IOptionsRoot
    {
        public string IdentityServiceUrl { get; set; }
    }
}
