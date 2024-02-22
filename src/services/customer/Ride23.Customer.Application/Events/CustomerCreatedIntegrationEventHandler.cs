using Microsoft.Extensions.Logging;
using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Consumers;
public class CustomerCreatedIntegrationEventHandler : IKafkaMessageHandler<string, CustomerCreatedIntegrationEvent>
{
    private readonly ILogger<CustomerCreatedIntegrationEventHandler> _logger;

    public CustomerCreatedIntegrationEventHandler(ILogger<CustomerCreatedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(string key, CustomerCreatedIntegrationEvent value)
    {
        Console.WriteLine($"Customer Id: {value.CustomerId} Customer Name: {value.CustomerName}");
    }
}
