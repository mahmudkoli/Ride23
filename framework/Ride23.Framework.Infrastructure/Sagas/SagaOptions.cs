using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Infrastructure.Sagas;
public class SagaOptions : IOptionsRoot
{
    public string QueueName { get; set; } = default!;
    public string TransportConnString { get; set; } = default!;
    public string PersistenceConnString { get; set; } = default!;
}
