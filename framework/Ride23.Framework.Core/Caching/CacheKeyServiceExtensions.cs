using Ride23.Framework.Core.Domain;

namespace Ride23.Framework.Core.Caching;

public static class CacheKeyServiceExtensions
{
    public static string GetCacheKey<TEntity>(this ICacheKeyService cacheKeyService, object id)
    where TEntity : IBaseEntity =>
        cacheKeyService.GetCacheKey(typeof(TEntity).Name, id);
}