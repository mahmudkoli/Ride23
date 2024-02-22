using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Infrastructure.Messaging.Producer;

public class KafkaProducer<TEvent> : IDisposable where TEvent : IEvent
{
    private readonly IProducer<string, TEvent> _producer;
    private readonly string _topic;

    public KafkaProducer(
        IOptions<KafkaProducerConfig> topicOptions, 
        IProducer<string, TEvent> producer)
    {
        _topic = topicOptions.Value.Topic;
        _producer = producer;
    }

    public async Task ProduceAsync(string key, TEvent value, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(_topic, new Message<string, TEvent> { Key = key, Value = value }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}