using Ride23.Framework.Core.Caching;

namespace Ride23.Framework.Infrastructure.Caching;

public class CacheKeyService : ICacheKeyService
{
    public CacheKeyService()
    {

    }

    public string GetCacheKey(string name, object id)
    {
        return $"{name}-{id}";
    }
}