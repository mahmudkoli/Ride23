using Microsoft.Extensions.Logging;
using Ride23.Event.Inventory;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Inventory.Application.Events;
public class InventoryCreatedIntegrationEventHandler : IKafkaMessageHandler<InventoryCreatedIntegrationEvent>
{
    private readonly ILogger<InventoryCreatedIntegrationEventHandler> _logger;

    public InventoryCreatedIntegrationEventHandler(ILogger<InventoryCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, InventoryCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
