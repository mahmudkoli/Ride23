using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ride23.Customer.Application;
using Ride23.Customer.Application.Events;
using Ride23.Customer.Application.Customers;
using Ride23.Customer.Domain.Customers.Events;
using Ride23.Customer.Infrastructure.Persistence;
using Ride23.Customer.Infrastructure.Repositories;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Infrastructure;
using Ride23.Framework.Infrastructure.Auth.OpenId;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging;
using Ride23.Framework.Infrastructure.Options;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Customer.Infrastructure;
public static class Extensions
{
    public static void AddCustomerInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(CustomerApplication).Assembly;
        var dbContextAssembly = typeof(CustomerDbContext).Assembly;
        var policyNames = new List<string> { "customer:read", "customer:write" };

        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();

        var config = builder.Configuration;
        var kafkaOptions = builder.Services.BindValidateReturn<KafkaOptions>(config);

        builder.Services.AddKafkaMessageBus();

        builder.Services.AddKafkaProducer<CustomerCreatedIntegrationEvent>(p =>
        {
            p.Topic = "customers";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddKafkaConsumer<CustomerCreatedIntegrationEvent, CustomerCreatedIntegrationEventHandler>(p =>
        {
            p.Topic = "customers";
            p.GroupId = "customers_group";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddEFCoreDbContext<CustomerDbContext>(builder.Configuration, dbContextAssembly);
        builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
    }
    public static void UseCustomerInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
