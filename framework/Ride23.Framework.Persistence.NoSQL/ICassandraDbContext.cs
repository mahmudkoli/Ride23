using Cassandra;

namespace Ride23.Framework.Persistence.NoSQL
{
    public interface ICassandraDbContext : IDisposable
    {
        ISession Session { get; }
    }
}
