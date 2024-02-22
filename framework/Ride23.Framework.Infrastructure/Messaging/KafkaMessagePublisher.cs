using Ride23.Framework.Core.Messaging;
using Ride23.Framework.Infrastructure.Messaging.Producer;

namespace Ride23.Framework.Infrastructure.Messaging;

public class KafkaMessagePublisher<Tk, Tv> : IKafkaMessagePublisher<Tk, Tv>
{
    public readonly KafkaProducer<Tk, Tv> _producer;
    
    public KafkaMessagePublisher(KafkaProducer<Tk, Tv> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(Tk key, Tv message)
    {
        await _producer.ProduceAsync(key, message);
    }
}