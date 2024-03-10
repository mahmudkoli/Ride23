using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;
using Ride23.Event.Inventory;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Infrastructure;
using Ride23.Framework.Infrastructure.Auth.OpenId;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging;
using Ride23.Framework.Infrastructure.Options;
using Ride23.Framework.Infrastructure.Sagas;
using Ride23.Framework.Persistence.EFCore;
using Ride23.Inventory.Application;
using Ride23.Inventory.Application.Events;
using Ride23.Inventory.Application.Inventories;
using Ride23.Inventory.Application.Sagas;
using Ride23.Inventory.Infrastructure.Persistence;
using Ride23.Inventory.Infrastructure.Repositories;
using Ride23.Saga.Order;

namespace Ride23.Inventory.Infrastructure;
public static class Extensions
{
    public static void AddInventoryInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(InventoryApplication).Assembly;
        var dbContextAssembly = typeof(InventoryDbContext).Assembly;
        var policyNames = new List<string> { "inventory:read", "inventory:write" };

        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();

        var config = builder.Configuration;
        var kafkaOptions = builder.Services.BindValidateReturn<KafkaOptions>(config);

        builder.Services.AddKafkaMessageBus();

        builder.Services.AddKafkaProducer<InventoryCreatedIntegrationEvent>(p =>
        {
            p.Topic = "inventories";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddKafkaConsumer<InventoryCreatedIntegrationEvent, InventoryCreatedIntegrationEventHandler>(p =>
        {
            p.Topic = "inventories";
            p.GroupId = "inventories_group";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddEFCoreDbContext<InventoryDbContext>(builder.Configuration, dbContextAssembly);
        builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();

        AddSaga(builder, config);
    }

    private static void AddSaga(WebApplicationBuilder builder, ConfigurationManager config)
    {
        var queueNameMappings = new Dictionary<Type, string>
        {
            { typeof(IOrderMap), "order-queue" },
            { typeof(IInventoryMap), "inventory-queue" }
        };
        var routingConfig = new Dictionary<Type, string>();
        foreach (var mapping in queueNameMappings)
        {
            var implementingTypes = typeof(OrderProcessingSagaData).Assembly.GetTypes()
                .Where(t => mapping.Key.IsAssignableFrom(t) && !t.IsInterface)
                .ToList();

            foreach (var implementingType in implementingTypes)
            {
                routingConfig[implementingType] = mapping.Value;
            }
        }

        builder.Services.AddSagaService<InventorySagaHandlers>(config, routingConfig, async bus =>
        {
            //await bus.Subscribe<InventoryReservationFailedEvent>();
            //await bus.Subscribe<InventoryReservedEvent>();
            //await bus.Subscribe<OrderCancelledEvent>();
        });

        //var sagaOptions = builder.Services.BindValidateReturn<SagaOptions>(builder.Configuration);
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //builder.Services.AddRebus(
        //    config => config
        //        .Logging(l => l.ColoredConsole(minLevel: LogLevel.Info))
        //        .Routing(r =>
        //        {
        //            var routing = r.TypeBased();
        //            routing.Map<InventoryReservedEvent>("inventory-queue");
        //            routing.Map<OrderProcessingSuccessEvent>("order-queue");
        //        })
        //        .Transport(t =>
        //            t.UseRabbitMq(
        //                sagaOptions.TransportConnString,
        //                "inventory-queue"))
        //        .Sagas(s =>
        //            s.StoreInPostgres(
        //                sagaOptions.PersistenceConnString,
        //                dataTableName: "Sagas",
        //                indexTableName: "SagaIndexes"))
        //        .Timeouts(t =>
        //            t.StoreInPostgres(
        //        sagaOptions.PersistenceConnString,
        //                tableName: "Timeouts"))
        //        //,
        //        //onCreated: async bus =>
        //        //{
        //        //    await bus.Subscribe<InventoryReservedEvent>();
        //        //    //await bus.Subscribe<ReserveInventoryCommand>();
        //        //}
        //    );

        //builder.Services.AutoRegisterHandlersFromAssemblyOf<InventorySagaHandlers>();
    }

    public static void UseInventoryInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
