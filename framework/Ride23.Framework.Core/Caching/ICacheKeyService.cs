namespace Ride23.Framework.Core.Caching;

public interface ICacheKeyService
{
    public string GetCacheKey(string name, object id);
}