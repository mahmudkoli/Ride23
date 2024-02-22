using Confluent.Kafka;
using Newtonsoft.Json;
using Ride23.Framework.Core.Events;
using System.Text;

namespace Ride23.Framework.Infrastructure.Messaging;

internal sealed class KafkaDeserializer<TEvent> : IDeserializer<TEvent> where TEvent : IEvent
{
    public TEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (typeof(TEvent) == typeof(Null))
        {
            if (data.Length > 0)
                throw new ArgumentException("The data is null not null.");
            return default;
        }

        if (typeof(TEvent) == typeof(Ignore))
            return default;

        var dataJson = Encoding.UTF8.GetString(data);

        return JsonConvert.DeserializeObject<TEvent>(dataJson);
    }
}