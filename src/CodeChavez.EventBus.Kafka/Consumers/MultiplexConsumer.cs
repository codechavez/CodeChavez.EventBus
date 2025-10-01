using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CodeChavez.EventBus.Kafka.Consumers;

public class MultiplexConsumer<TEvent> : BackgroundService where TEvent : class, INotification
{
    private readonly string _topic;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<MultiplexConsumer<TEvent>> _logger;

    private readonly int _batchSize;
    private readonly TimeSpan _pollTimeout;
    private readonly int _maxParallelism;

    public MultiplexConsumer(
        string topic,
        ConsumerOptions config,
        IServiceScopeFactory scopeFactory,
        ILogger<MultiplexConsumer<TEvent>> logger,
        IConfiguration configuration
        )
    {
        _topic = topic;
        _scopeFactory = scopeFactory;
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _logger = logger;

        _batchSize = configuration.GetValue<int>("EVENTBUS:Consumer:MaxConcurrency");
        _maxParallelism = configuration.GetValue<int>("EVENTBUS:Consumer:MaxParalleism");
        _pollTimeout = TimeSpan.FromMicroseconds(configuration.GetValue<int>("EVENTBUS:Consumer:PollTimeout"));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Listing for {_topic}");
        _consumer.Subscribe(_topic);

        using var scope = _scopeFactory.CreateScope();
        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = new List<ConsumeResult<string, string>>(_batchSize);
                while (messages.Count < _batchSize)
                {
                    var message = _consumer.Consume(_pollTimeout);
                    if (message != null)
                    {
                        messages.Add(message);
                    }
                    else
                    {
                        break; // No more messages this cycle
                    }
                }

                if (messages.Count == 0)
                    continue;

                await Parallel.ForEachAsync(messages, new ParallelOptions
                {
                    MaxDegreeOfParallelism = _maxParallelism,
                    CancellationToken = cancellationToken
                },
                async (kafkaMessage, token) =>
                {
                    try
                    {
                        var kafkaValue = kafkaMessage.Message.Value;
                        var @event = JsonSerializer.Deserialize<TEvent>(kafkaValue);

                        if (@event is null)
                            return;

                        using var scope = _scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Publish(@event, cancellationToken);
                    }
                    catch (OperationCanceledException) { /* normal during shutdown */ }
                    catch (Exception ex)
                    {
                        _logger.LogError($"⚠️ Error processing message: {ex.Message}");
                    }
                });
            }
            catch (OperationCanceledException) { /* shutting down */ }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Kafka error on topic {_topic}: {ex.Message}");
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}