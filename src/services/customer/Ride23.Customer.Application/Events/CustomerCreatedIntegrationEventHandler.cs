using Microsoft.Extensions.Logging;
using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Events;
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
