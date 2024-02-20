using Confluent.Kafka;

namespace Ride23.Framework.Infrastructure.Messaging.Consumer;

public class KafkaConsumerConfig<Tk, Tv> : ConsumerConfig
{
    public string Topic { get; set; }

    public KafkaConsumerConfig()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoOffsetStore = false;
    }
}