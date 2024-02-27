using Microsoft.Extensions.Logging;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class LocationCreatedIntegrationEventHandler : IKafkaMessageHandler<LocationCreatedIntegrationEvent>
{
    private readonly ILogger<LocationCreatedIntegrationEventHandler> _logger;

    public LocationCreatedIntegrationEventHandler(ILogger<LocationCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, LocationCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
