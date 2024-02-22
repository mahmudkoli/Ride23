using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Events;
using Ride23.Framework.Core.Messaging;
using Ride23.Framework.Infrastructure.Messaging.Consumer;
using Ride23.Framework.Infrastructure.Messaging.Producer;

namespace Ride23.Framework.Infrastructure.Messaging;

public static class Extensions
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
        => serviceCollection.AddSingleton(typeof(IKafkaMessagePublisher<>), typeof(KafkaMessagePublisher<>));

    public static IServiceCollection AddKafkaConsumer<TEvent, TEventHandler>(this IServiceCollection services,
        Action<KafkaConsumerConfig> configAction) 
        where TEvent : IEvent 
        where TEventHandler : class, IKafkaMessageHandler<TEvent>
    {
        services.AddScoped<IKafkaMessageHandler<TEvent>, TEventHandler>();

        services.AddHostedService<BackgroundKafkaConsumer<TEvent>>();

        services.Configure(configAction);

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TEvent>(this IServiceCollection services,
        Action<KafkaProducerConfig> configAction) where TEvent : IEvent
    {
        services.AddConfluentKafkaProducer<TEvent>();

        services.AddSingleton<KafkaProducer<TEvent>>();

        services.Configure(configAction);

        return services;
    }

    private static IServiceCollection AddConfluentKafkaProducer<TEvent>(this IServiceCollection services) where TEvent : IEvent
    {
        services.AddSingleton(
            sp =>
            {
                var config = sp.GetRequiredService<IOptions<KafkaProducerConfig>>();

                var builder = new ProducerBuilder<string, TEvent>(config.Value).SetValueSerializer(new KafkaSerializer<TEvent>());

                return builder.Build();
            });

        return services;
    }
}