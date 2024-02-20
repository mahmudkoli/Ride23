using Ride23.Framework.Core.Caching;
using Ride23.Framework.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ride23.Framework.Infrastructure.Caching;
public static class Extensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheOptions = services.BindValidateReturn<CachingOptions>(configuration);
        if (cacheOptions.EnableDistributedCaching)
        {
            if (cacheOptions.PreferRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheOptions.RedisURL;
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    {
                        AbortOnConnectFail = true,
                        EndPoints = { cacheOptions.RedisURL }
                    };
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.AddTransient<ICacheService, DistributedCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddTransient<ICacheService, InMemoryCacheService>();
        }

        return services;
    }
}
