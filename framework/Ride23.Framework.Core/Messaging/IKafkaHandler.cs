namespace Ride23.Framework.Core.Messaging;

public interface IKafkaHandler<Tk, Tv>
{
    Task HandleAsync(Tk key, Tv value);
}