using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Infrastructure.Messaging
{
    public class KafkaOptions : IOptionsRoot
    {
        public string BootstrapServers { get; set; } = default!;
    }
}
