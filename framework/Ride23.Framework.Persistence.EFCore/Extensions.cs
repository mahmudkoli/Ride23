using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSFA23.Application.Common.Persistence;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Infrastructure.Options;
using System.Reflection;

namespace Ride23.Framework.Persistence.EFCore;
public static class Extensions
{
    public static IServiceCollection AddEFCoreDbContext<TContext>(
        this IServiceCollection services, IConfiguration configuration, Assembly dbContextAssembly)
        where TContext : EFCoreDbContext
    {
        return services.AddEFCoreDbContext<TContext, TContext>(configuration, dbContextAssembly);
    }

    public static IServiceCollection AddEFCoreDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services, IConfiguration configuration, Assembly dbContextAssembly)
        where TContextService : IEFCoreDbContext
        where TContextImplementation : EFCoreDbContext, TContextService
    {
        var options = services.BindValidateReturn<EFCoreOptions>(configuration);
        if (string.IsNullOrEmpty(options.DBProvider)) throw new ArgumentNullException(nameof(options.DBProvider));
        if (string.IsNullOrEmpty(options.ConnectionString)) throw new ArgumentNullException(nameof(options.ConnectionString));
        services.AddDbContext<TContextImplementation>(m => m.UseDatabase(options.DBProvider, options.ConnectionString, dbContextAssembly, options.DefaultSchema));
        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));
        services.AddScoped<IEFCoreDbContext>(sp => sp.GetRequiredService<TContextService>());
        services.AddTransient(typeof(IRepository<,>), typeof(EFCoreRepository<,>));
        services.AddTransient(typeof(IUnitOfWork), typeof(EFCoreUnitOfWork));
        services.AddTransient(typeof(IRepositoryWithEvents<,>), typeof(EFCoreEventAddingRepositoryDecorator<,>));

        return services;
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString, Assembly dbContextAssembly, string schemaName)
    {
        switch (dbProvider.ToLowerInvariant())
        {
            case DbProviderKeys.Npgsql:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e =>
                     { 
                         e.MigrationsAssembly(dbContextAssembly.FullName);
                         e.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schemaName);
                     });

            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}
