namespace Ride23.Framework.Core.Messaging;

public interface IKafkaMessagePublisher<Tk, Tv>
{
    Task PublishAsync(Tk key, Tv message);
}