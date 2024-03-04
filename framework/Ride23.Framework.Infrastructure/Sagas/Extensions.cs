using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Infrastructure.Sagas;
public static class Extensions
{
    public static IServiceCollection AddSagaService<TMessage, TMessageHandler>(this IServiceCollection services, IConfiguration configuration, Func<IBus, Task>? onCreated = null)
        where TMessage : class
        where TMessageHandler : class
    {
        var sagaOptions = services.BindValidateReturn<SagaOptions>(configuration);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddRebus(
            config => config
                .Routing(r =>
                    r.TypeBased().MapAssemblyOf<TMessage>(sagaOptions.QueueName))
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
