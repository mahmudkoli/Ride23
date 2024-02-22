using Microsoft.Extensions.Logging;
using Ride23.Customer.Domain.Customers.Events;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Consumers;
public class CustomerCreatedDomainEventHandler : EventNotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly ILogger<CustomerCreatedDomainEventHandler> _logger;
    private readonly IKafkaMessagePublisher<string, CustomerCreatedIntegrationEvent> _messagePublisher;

    public CustomerCreatedDomainEventHandler(
        ILogger<CustomerCreatedDomainEventHandler> logger
        , IKafkaMessagePublisher<string, CustomerCreatedIntegrationEvent> messagePublisher
        )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public override Task Handle(CustomerCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Created On {CreationDate}", @event.GetType().Name, @event.CreationDate);
        _messagePublisher.PublishAsync(@event.Id.ToString(), new CustomerCreatedIntegrationEvent(@event.IdentityGuid, @event.Id, @event.CustomerName));
        return Task.CompletedTask;
    }
}
