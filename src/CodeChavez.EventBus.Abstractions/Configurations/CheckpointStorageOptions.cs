namespace CodeChavez.EventBus.Abstractions.Configurations;

/// <summary>
/// Configuration options for checkpoint storage, which is used to track the progress of message processing and ensure at-least-once delivery semantics.
/// </summary>
public record CheckpointStorageOptions
{
    public string CheckpointName { get; set; } = string.Empty;
    public string CheckpointConnectionString { get; set; } = string.Empty;
}