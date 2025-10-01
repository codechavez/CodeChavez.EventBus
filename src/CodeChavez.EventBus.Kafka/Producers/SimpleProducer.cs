using CodeChavez.EventBus.Abstractions.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CodeChavez.EventBus.Kafka.Producers;

public class SimpleProducer : IEventBusProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<SimpleProducer> _logger;

    public SimpleProducer(
        ProducerOptions options,
        ILogger<SimpleProducer> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            ClientId = options.ClientId ?? $"kafka.producer-{Guid.NewGuid().ToString().Replace("-", "")}"
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string? topic = null, CancellationToken cancellationToken = default) where TEvent : class
    {
        if (string.IsNullOrEmpty(topic))
            topic = typeof(TEvent).Name;

        var json = JsonSerializer.Serialize(@event);
        _logger.LogInformation(json);

        try
        {
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json }, cancellationToken);
            _logger.LogInformation($"✅ {typeof(TEvent).Name} via {topic}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}