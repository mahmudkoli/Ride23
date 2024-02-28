using Ride23.Event.Driver;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class DriverCreatedIntegrationEventHandler : IKafkaMessageHandler<DriverCreatedIntegrationEvent>
{
    private readonly ILogger<DriverCreatedIntegrationEventHandler> _logger;

    public DriverCreatedIntegrationEventHandler(ILogger<DriverCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, DriverCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
