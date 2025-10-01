using CodeChavez.EventBus.Abstractions.Interfaces;
using CodeChavez.EventBus.Kafka.Consumers;
using CodeChavez.EventBus.Kafka.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeChavez.EventBus.Kafka;

public static class EventBusKafkaModule
{
    public static IServiceCollection AddKafkaConsumerOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var configs = configuration.GetSection("EVENTBUS");
        var servers = configuration.GetSection("EVENTBUS:Consumer:Servers").Get<string[]>();

        services.AddSingleton(new ConsumerOptions
        {
            BootstrapServers = string.Join(',', servers),
            GroupId = configs.GetValue<string>("Consumer:Group"),
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = true,
            ClientId = $"{configs.GetValue<string>("Consumer:Client")}.{DateTime.Now.Ticks}"
        });

        return services;
    }

    public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
    {
        var configs = configuration.GetSection("EVENTBUS");
        var servers = configuration.GetSection("EVENTBUS:Producer:Servers").Get<string[]>();

        services.AddSingleton(new ProducerOptions
        {
            BootstrapServers = string.Join(',', servers),
            ClientId = $"{configs.GetValue<string>("Producer:Client")}.{DateTime.Now.Ticks}"
        });

        services.AddSingleton<IEventBusProducer, SimpleProducer>();

        return services;
    }

    public static IServiceCollection AddKafkaProducer(this IServiceCollection services, string producerName, IConsumerProducerOptions options)
    {
        services.AddKeyedSingleton<IEventBusProducer>(producerName, (sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<SimpleProducer>>();
            var producerOptions = new ProducerOptions
            {
                BootstrapServers = string.Join(',', options.Servers),
                ClientId = $"{options.Client}.{DateTime.Now.Ticks}"
            };

            return new SimpleProducer(producerOptions, logger);
        });

        return services;
    }
}
