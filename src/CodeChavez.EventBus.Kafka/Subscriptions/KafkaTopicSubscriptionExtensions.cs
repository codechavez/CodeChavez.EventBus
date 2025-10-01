using CodeChavez.EventBus.Kafka.Consumers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeChavez.EventBus.Kafka.Subscriptions;

public static class KafkaTopicSubscriptionExtensions
{
    public static IServiceCollection AddKafkaSubscription<TEvent>(this IServiceCollection services) where TEvent : class, INotification
    {
        services.AddHostedService(sp =>
        {
            return new MultiplexConsumer<TEvent>(
                typeof(TEvent).Name,
                sp.GetRequiredService<ConsumerOptions>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                sp.GetRequiredService<ILogger<MultiplexConsumer<TEvent>>>(),
                sp.GetRequiredService<IConfiguration>()
            );
        });

        return services;
    }
}