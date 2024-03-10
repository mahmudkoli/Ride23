using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;
using Ride23.Framework.Infrastructure.Options;
using System.Reflection;

namespace Ride23.Framework.Infrastructure.Sagas;
public static class Extensions
{
    public static IServiceCollection AddSagaService<TMessageHandler>(this IServiceCollection services, IConfiguration configuration, Dictionary<Type, string> routingConfig, Func<IBus, Task>? onCreated = null)
        where TMessageHandler : class
    {
        var sagaOptions = services.BindValidateReturn<SagaOptions>(configuration);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddRebus(
            config => config
                .Logging(l => l.ColoredConsole(minLevel: LogLevel.Info))
                .Routing(r =>
                {
                    var typeBasedRouting = r.TypeBased();

                    foreach (var messageType in routingConfig)
                    {
                        typeBasedRouting.Map(messageType.Key, messageType.Value);
                    }
                })
                .Transport(t =>
                    t.UseRabbitMq(
                        sagaOptions.TransportConnString, 
                        sagaOptions.QueueName))
                .Sagas(s =>
                    s.StoreInPostgres(
                        sagaOptions.PersistenceConnString,
                        dataTableName: "Sagas",
                        indexTableName: "SagaIndexes"))
                .Timeouts(t =>
                    t.StoreInPostgres(
                        sagaOptions.PersistenceConnString,
                        tableName: "Timeouts"))
                ,
                onCreated: onCreated ?? (_ => Task.CompletedTask)
        );

        services.AutoRegisterHandlersFromAssemblyOf<TMessageHandler>();

        return services;
    }
}
