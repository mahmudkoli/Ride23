using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Framework.Infrastructure.Messaging.Consumer;

public class BackgroundKafkaConsumer<TK, TV> : BackgroundService
{
    private readonly KafkaConsumerConfig<TK, TV> _config;
    private IKafkaHandler<TK, TV> _handler;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundKafkaConsumer(IOptions<KafkaConsumerConfig<TK, TV>> config,
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
            _handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TK, TV>>();

            var builder = new ConsumerBuilder<TK, TV>(_config).SetValueDeserializer(new KafkaDeserializer<TV>());

            using (IConsumer<TK, TV> consumer = builder.Build())
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