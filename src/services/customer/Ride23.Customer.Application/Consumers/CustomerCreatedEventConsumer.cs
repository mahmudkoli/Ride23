using Microsoft.Extensions.Logging;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Consumers;
public class CustomerCreatedEventConsumer : IKafkaHandler<string, CustomerDto>
{
    private readonly ILogger<CustomerCreatedEventConsumer> _logger;

    public CustomerCreatedEventConsumer(ILogger<CustomerCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(string key, CustomerDto value)
    {
        Console.WriteLine($"Customer Id: {value.Id} Customer Name: {value.Name}");
    }
}
