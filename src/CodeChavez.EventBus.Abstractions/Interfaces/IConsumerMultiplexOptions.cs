namespace CodeChavez.EventBus.Abstractions.Interfaces;

public interface IConsumerMultiplexOptions
{
    int MaxConcurrency { get; set; }

    int PollTimeout { get; set; }

    int MaxParallelism { get; set; }
}