using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ride23.Framework.Core.Messaging;
using Ride23.Framework.Infrastructure.Messaging.Consumer;
using Ride23.Framework.Infrastructure.Messaging.Producer;

namespace Ride23.Framework.Infrastructure.Messaging;

public static class Extensions
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
        => serviceCollection.AddSingleton(typeof(IKafkaMessagePublisher<,>), typeof(KafkaMessagePublisher<,>));

    public static IServiceCollection AddKafkaConsumer<Tk, Tv, THandler>(this IServiceCollection services,
        Action<KafkaConsumerConfig<Tk, Tv>> configAction) where THandler : class, IKafkaMessageHandler<Tk, Tv>
    {
        services.AddScoped<IKafkaMessageHandler<Tk, Tv>, THandler>();

        services.AddHostedService<BackgroundKafkaConsumer<Tk, Tv>>();

        services.Configure<KafkaConsumerConfig<Tk, Tv>>(configAction);

        return services;
    }

    public static IServiceCollection AddKafkaProducer<Tk, Tv>(this IServiceCollection services,
        Action<KafkaProducerConfig<Tk, Tv>> configAction)
    {
        services.AddConfluentKafkaProducer<Tk, Tv>();

        services.AddSingleton<KafkaProducer<Tk, Tv>>();

        services.Configure(configAction);

        return services;
    }

    private static IServiceCollection AddConfluentKafkaProducer<Tk, Tv>(this IServiceCollection services)
    {
        services.AddSingleton(
            sp =>
            {
                var config = sp.GetRequiredService<IOptions<KafkaProducerConfig<Tk, Tv>>>();

                var builder = new ProducerBuilder<Tk, Tv>(config.Value).SetValueSerializer(new KafkaSerializer<Tv>());

                return builder.Build();
            });

        return services;
    }
}