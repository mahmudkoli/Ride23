using Cassandra;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;

namespace Ride23.Framework.Persistence.NoSQL
{
    public class CassandraDbContext : ICassandraDbContext
    {
        public ISession Session { get; }

        public CassandraDbContext(IOptions<CassandraOptions> options)
        {
            Cluster cluster = Cluster.Builder()
                                     .AddContactPoint(options.Value.ContactPoint)
                                     .WithPort(options.Value.Port)
                                     .Build();
            Session = cluster.Connect(options.Value.Keyspace);
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
