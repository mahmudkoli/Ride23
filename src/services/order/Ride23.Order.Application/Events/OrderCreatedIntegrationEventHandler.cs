using Microsoft.Extensions.Logging;
using Ride23.Event.Order;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Order.Application.Events;
public class OrderCreatedIntegrationEventHandler : IKafkaMessageHandler<OrderCreatedIntegrationEvent>
{
    private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;

    public OrderCreatedIntegrationEventHandler(ILogger<OrderCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, OrderCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
