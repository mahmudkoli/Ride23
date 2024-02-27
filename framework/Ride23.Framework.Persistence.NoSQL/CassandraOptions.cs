using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Persistence.NoSQL
{
    public class CassandraOptions : IOptionsRoot
    {
        public string ContactPoint { get; set; } = null!;
        public int Port { get; set; }
        public string Keyspace { get; set; } = null!;
    }
}
