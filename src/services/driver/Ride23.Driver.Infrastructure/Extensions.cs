using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ride23.Driver.Application;
using Ride23.Driver.Application.Common;
using Ride23.Driver.Application.Drivers;
using Ride23.Driver.Application.Events;
using Ride23.Driver.Infrastructure.Persistence;
using Ride23.Driver.Infrastructure.Repositories;
using Ride23.Driver.Infrastructure.Service;
using Ride23.Event.Driver;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Infrastructure;
using Ride23.Framework.Infrastructure.Auth.OpenId;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging;
using Ride23.Framework.Infrastructure.Options;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Driver.Infrastructure;
public static class Extensions
{
    public static void AddDriverInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(DriverApplication).Assembly;
        var dbContextAssembly = typeof(DriverDbContext).Assembly;
        var policyNames = new List<string> { "driver:read", "driver:write" };

        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();
        builder.Services
            .AddScoped<IUserService, UserService>()
            .AddGrpcClient<UserGrpc.User.UserClient>(o =>
        {
            o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:IdentityServiceUrl") ?? "");
        });
        var config = builder.Configuration;
        var kafkaOptions = builder.Services.BindValidateReturn<KafkaOptions>(config);

        builder.Services.AddKafkaMessageBus();

        builder.Services.AddKafkaProducer<DriverCreatedIntegrationEvent>(p =>
        {
            p.Topic = "drivers";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddKafkaConsumer<DriverCreatedIntegrationEvent, DriverCreatedIntegrationEventHandler>(p =>
        {
            p.Topic = "drivers";
            p.GroupId = "drivers_group";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddEFCoreDbContext<DriverDbContext>(builder.Configuration, dbContextAssembly);
        builder.Services.AddTransient<IDriverRepository, DriverRepository>();
    }
    public static void UseDriverInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
