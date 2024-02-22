using Microsoft.Extensions.Logging;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;
using Ride23.Framework.Infrastructure.Events;
using Ride23.Framework.Infrastructure.Messaging.Producer;

namespace Ride23.Framework.Infrastructure.Messaging;

public class KafkaMessagePublisher<TEvent> : IKafkaMessagePublisher<TEvent> where TEvent : IEvent
{
    public readonly KafkaProducer<TEvent> _producer;
    private readonly ILogger<EventPublisher> _logger;

    public KafkaMessagePublisher(
        KafkaProducer<TEvent> producer, 
        ILogger<EventPublisher> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task PublishAsync(string key, TEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing Event : {event} on {DateTime} for Event: {Id} Created On {CreationDate}", 
            @event.GetType().Name, DateTime.UtcNow, @event.Id, @event.CreationDate);
        await _producer.ProduceAsync(key, @event, cancellationToken);
    }
}