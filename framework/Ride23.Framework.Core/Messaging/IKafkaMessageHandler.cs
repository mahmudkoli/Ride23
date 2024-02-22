namespace Ride23.Framework.Core.Messaging;

public interface IKafkaMessageHandler<Tk, Tv>
{
    Task HandleAsync(Tk key, Tv value);
}