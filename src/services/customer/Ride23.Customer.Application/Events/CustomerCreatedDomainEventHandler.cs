using Microsoft.Extensions.Logging;
using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Events;
public class CustomerCreatedDomainEventHandler : EventNotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly ILogger<CustomerCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<CustomerCreatedIntegrationEvent> _messagePublisher;

    public CustomerCreatedDomainEventHandler(
        ILogger<CustomerCreatedDomainEventHandler> logger, 
        IKafkaMessagePublisher<CustomerCreatedIntegrationEvent> messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(CustomerCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}",
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.CustomerId.ToString(), new CustomerCreatedIntegrationEvent(@event.IdentityGuid, @event.CustomerId, @event.CustomerName), cancellationToken);
        return Task.CompletedTask;
    }
}
