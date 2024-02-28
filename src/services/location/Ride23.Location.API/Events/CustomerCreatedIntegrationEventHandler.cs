using Ride23.Event.Customer;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Location.API.Events;
public class CustomerCreatedIntegrationEventHandler : IKafkaMessageHandler<CustomerCreatedIntegrationEvent>
{
    private readonly ILogger<CustomerCreatedIntegrationEventHandler> _logger;

    public CustomerCreatedIntegrationEventHandler(ILogger<CustomerCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(string key, CustomerCreatedIntegrationEvent @event)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        return Task.CompletedTask;
    }
}
