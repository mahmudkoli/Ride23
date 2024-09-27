using Ride23.Event.Location;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class DriverLocationCreatedIntegrationEventHandler : IKafkaMessageHandler<DriverLocationCreatedIntegrationEvent>
{
    private readonly ILogger<DriverLocationCreatedIntegrationEventHandler> _logger;

    public DriverLocationCreatedIntegrationEventHandler(ILogger<DriverLocationCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, DriverLocationCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
