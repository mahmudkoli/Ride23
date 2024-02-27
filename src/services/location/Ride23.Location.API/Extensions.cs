using Ride23.Framework.Core.Events;
using Ride23.Framework.Infrastructure;
using Ride23.Framework.Infrastructure.Auth.OpenId;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging;
using Ride23.Framework.Infrastructure.Options;
using Ride23.Framework.Persistence.NoSQL;
using Ride23.Location.API.Events;
using Ride23.Location.API.Persistence;
using Ride23.Location.API.Repositories;
using Ride23.Location.API.Services;

namespace Ride23.Location.API;
public static class Extensions
{
    public static void AddLocationInfrastructure(this WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;
        var policyNames = new List<string> { "location:read", "location:write" };

        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(assembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();

        var config = builder.Configuration;
        var kafkaOptions = builder.Services.BindValidateReturn<KafkaOptions>(config);

        builder.Services.AddKafkaMessageBus();

        builder.Services.AddKafkaProducer<LocationCreatedIntegrationEvent>(p =>
        {
            p.Topic = "locations";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddKafkaConsumer<LocationCreatedIntegrationEvent, LocationCreatedIntegrationEventHandler>(p =>
        {
            p.Topic = "locations";
            p.GroupId = "locations_group";
            p.BootstrapServers = kafkaOptions.BootstrapServers;
        });

        builder.Services.AddCassandraDbContext<LocationDbContext>(builder.Configuration);
        builder.Services.AddTransient<ILocationRepository, LocationRepository>();
        builder.Services.AddScoped<ILocationService, LocationService>();
    }
    public static void UseLocationInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
