using System.ComponentModel.DataAnnotations;

namespace CodeChavez.EventBus.Abstractions.Configurations;

public record ServiceBusOptions
{
    public string ConnecitonString { get; set; } = string.Empty;

    /// <summary>
    /// The number of messages to prefetch from the broker in each poll. Must be between 1 and 10,000. Default is 5000.
    /// </summary>
    [Range(1, 10_000)]
    public ushort PrefetchCount { get; set; } = 5000;

    /// <summary>
    /// The maximum number of messages to process in a single batch. Must be between 1 and 10,000. Default is 1000.
    /// </summary>
    [Range(1, 10_000)]
    public ushort BatchSize { get; set; } = 1000;

    /// <summary>
    /// A unique identifier for the client instance. 
    /// </summary>
    public string ClientId { get; set; } = DateTime.UtcNow.Ticks.ToString();

    /// <summary>
    /// The consumer group ID to use when consuming messages. This should be the same for all instances of the consumer that are part of the same group, allowing them to share the load of processing messages from the topic.
    /// </summary>
    public string ConsumerGroup { get; set; } = string.Empty;

    /// <summary>
    /// The topic to subscribe to for consuming messages. This should match the topic that producers are publishing to.
    /// </summary>
    public required string Topic { get; set; } = string.Empty;

}
