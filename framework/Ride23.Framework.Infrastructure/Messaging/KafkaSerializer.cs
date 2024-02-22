using Confluent.Kafka;
using Newtonsoft.Json;
using Ride23.Framework.Core.Events;
using System.Text;


namespace Ride23.Framework.Infrastructure.Messaging;

internal sealed class KafkaSerializer<TEvent> : ISerializer<TEvent> where TEvent : IEvent
{
    public byte[] Serialize(TEvent data, SerializationContext context)
    {
        if (typeof(TEvent) == typeof(Null))
            return null;

        if (typeof(TEvent) == typeof(Ignore))
            throw new NotSupportedException("Not Supported.");

        var json = JsonConvert.SerializeObject(data);

        return Encoding.UTF8.GetBytes(json);
    }
}