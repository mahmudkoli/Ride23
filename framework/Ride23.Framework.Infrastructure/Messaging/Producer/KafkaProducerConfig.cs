using Confluent.Kafka;

namespace Ride23.Framework.Infrastructure.Messaging.Producer;

public class KafkaProducerConfig : ProducerConfig
{
    public string Topic { get; set; }
}