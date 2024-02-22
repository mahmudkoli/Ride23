using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Framework.Infrastructure.Messaging.Consumer;

public class BackgroundKafkaConsumer<TEvent> : BackgroundService where TEvent : IEvent
{
    private readonly KafkaConsumerConfig _config;
    private IKafkaMessageHandler<TEvent> _handler;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundKafkaConsumer(IOptions<KafkaConsumerConfig> config,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            _handler = scope.ServiceProvider.GetRequiredService<IKafkaMessageHandler<TEvent>>();

            var builder = new ConsumerBuilder<string, TEvent>(_config).SetValueDeserializer(new KafkaDeserializer<TEvent>());

            using (IConsumer<string, TEvent> consumer = builder.Build())
            {
                consumer.Subscribe(_config.Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

                    if (result != null)
                    {
                        await _handler.HandleAsync(result.Message.Key, result.Message.Value);

                        consumer.Commit(result);

                        consumer.StoreOffset(result);
                    }
                }
            }
        }
    }
}