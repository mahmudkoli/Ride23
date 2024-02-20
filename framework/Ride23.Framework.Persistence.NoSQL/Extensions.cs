using Ride23.Framework.Core.Database;
using Ride23.Framework.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ride23.Framework.Persistence.NoSQL
{
    public static class Extensions
    {
        public static IServiceCollection AddCassandraDbContext<TContext>(
            this IServiceCollection services, IConfiguration configuration)
            where TContext : CassandraDbContext
        {
            return services.AddCassandraDbContext<TContext, TContext>(configuration);
        }

        public static IServiceCollection AddCassandraDbContext<TContextService, TContextImplementation>(
            this IServiceCollection services, IConfiguration configuration)
            where TContextService : ICassandraDbContext
            where TContextImplementation : CassandraDbContext, TContextService
        {
            var options = services.BindValidateReturn<CassandraOptions>(configuration);
            if (string.IsNullOrEmpty(options.ContactPoint)) throw new ArgumentNullException(nameof(options.ContactPoint));
            if (string.IsNullOrEmpty(options.Keyspace)) throw new ArgumentNullException(nameof(options.Keyspace));
            services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
            services.AddScoped(typeof(TContextImplementation));
            services.AddScoped<ICassandraDbContext>(sp => sp.GetRequiredService<TContextService>());

            // You may want to add additional services specific to Cassandra here

            return services;
        }
    }
}
