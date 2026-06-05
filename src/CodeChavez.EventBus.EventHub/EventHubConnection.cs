using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using CodeChavez.EventBus.Abstractions.Configurations;
using CodeChavez.EventBus.Abstractions.Extensions;
using CodeChavez.EventBus.EventHub.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeChavez.EventBus.EventHub;

public class EventHubConnection : IEventBusConnection
{
    private readonly ILogger<EventHubConnection> _logger;
    private readonly EventBusOptions _eventHubOptions;
    private readonly CheckpointStorageOptions _checkpointOptions;

    public EventHubConnection(
        ILogger<EventHubConnection> logger,
        IOptions<EventBusOptions> eventHubOptions,
        IOptions<CheckpointStorageOptions> checkpointOptions)
    {
        _logger = logger;
        _eventHubOptions = eventHubOptions.Value;
        _checkpointOptions = checkpointOptions.Value;
    }

    public async Task<EventProcessorClient> GetProcessorClientAsync()
    {
        var blobContainerClient = new Azure.Storage.Blobs.BlobContainerClient(
            _checkpointOptions.CheckpointConnectionString,
            _checkpointOptions.CheckpointName);

        await blobContainerClient.CreateIfNotExistsAsync();

        var processor = new EventProcessorClient(
            blobContainerClient,
            _eventHubOptions.ConsumerGroup,
            _eventHubOptions.Hosts.GetString(),
            _eventHubOptions.Topic,
            new EventProcessorClientOptions
            {
                MaximumWaitTime = TimeSpan.FromSeconds(60),
                PrefetchCount = _eventHubOptions.PrefetchCount,
                LoadBalancingStrategy = LoadBalancingStrategy.Balanced,
                RetryOptions = new EventHubsRetryOptions
                {
                    Mode = EventHubsRetryMode.Exponential,
                    Delay = TimeSpan.FromSeconds(0.5),
                    MaximumDelay = TimeSpan.FromSeconds(30),
                    MaximumRetries = 3
                }
            });

        return processor;
    }
}
