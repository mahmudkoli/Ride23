namespace Ride23.Framework.Core.Messaging;

public interface IKafkaMessageBus<Tk, Tv>
{
    Task PublishAsync(Tk key, Tv message);
}