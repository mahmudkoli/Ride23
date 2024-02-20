using Confluent.Kafka;

namespace Ride23.Framework.Infrastructure.Messaging.Producer;

public class KafkaProducerConfig<Tk, Tv> : ProducerConfig
{
    public string Topic { get; set; }
}