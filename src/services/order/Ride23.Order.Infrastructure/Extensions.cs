using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ride23.Order.Application;
using Ride23.Order.Application.Orders;
using Ride23.Order.Application.Events;
using Ride23.Order.Infrastructure.Persistence;
using Ride23.Order.Infrastructure.Repositories;
using Ride23.Event.Order;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Infrastructure;
using Ride23.Framework.Infrastructure.Auth.OpenId;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging;
using Ride23.Framework.Infrastructure.Options;
using Ride23.Framework.Persistence.EFCore;
using Ride23.Framework.Infrastructure.Sagas;
using Ride23.Order.Application.Orders.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Infrastructure;
public static class Extensions
{
    public static void AddOrderInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(OrderApplication).Assembly;
        var dbContextAssembly = typeof(OrderDbContext).Assembly;
        var policyNames = new List<string> { "order:read", "order:write" };

        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();

        var config = builder.Configuration;
        var kafkaOptions = builder.Services.BindValidateReturn<KafkaOptions>(config);

        builder.Services.AddKafkaMessageBus();

        builder.Services.AddKafkaProducer<OrderCreatedIntegrationEvent>(p =>
        {
            p.Topic = "orders";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddKafkaConsumer<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>(p =>
        {
            p.Topic = "orders";
            p.GroupId = "orders_group";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddEFCoreDbContext<OrderDbContext>(builder.Configuration, dbContextAssembly);
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();

        builder.Services.AddSagaService<OrderCreateSagaData, OrderSaga>(config, async bus =>
        {
            await bus.Subscribe<OrderConfirmationEmailSent>();
            await bus.Subscribe<OrderPaymentRequestSent>();
        });
    }
    public static void UseOrderInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
