using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Sagas;

public class NotificationSagaHandlers :
        Saga<OrderProcessingSagaData>,
        IHandleMessages<NotificationSentEvent>
{
    private readonly IBus _bus;
    private readonly ILogger<NotificationSagaHandlers> _logger;

    public NotificationSagaHandlers(IBus bus, ILogger<NotificationSagaHandlers> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderProcessingSagaData> config)
    {
        config.Correlate<NotificationSentEvent>(msg => msg.OrderId, data => data.OrderId);
    }

    public async Task Handle(NotificationSentEvent message)
    {
        _logger.LogInformation("Handling NotificationSentEvent for OrderId: {OrderId}", message.OrderId);
        Data.NotificationSent = true;
    }
}
