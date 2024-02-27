using Microsoft.Extensions.Options;
using Ride23.Framework.Persistence.NoSQL;

namespace Ride23.Location.API.Persistence;

public class LocationDbContext : CassandraDbContext
{
    public LocationDbContext(IOptions<CassandraOptions> options) : base(options)
    {
    }
}
