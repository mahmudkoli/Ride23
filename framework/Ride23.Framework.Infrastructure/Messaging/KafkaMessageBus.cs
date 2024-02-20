using Ride23.Framework.Core.Messaging;
using Ride23.Framework.Infrastructure.Messaging.Producer;

namespace Ride23.Framework.Infrastructure.Messaging;

public class KafkaMessageBus<Tk, Tv> : IKafkaMessageBus<Tk, Tv>
{
    public readonly KafkaProducer<Tk, Tv> _producer;
    
    public KafkaMessageBus(KafkaProducer<Tk, Tv> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(Tk key, Tv message)
    {
        await _producer.ProduceAsync(key, message);
    }
}