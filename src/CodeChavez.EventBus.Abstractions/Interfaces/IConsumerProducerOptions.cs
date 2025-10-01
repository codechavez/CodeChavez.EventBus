namespace CodeChavez.EventBus.Abstractions.Interfaces;

public interface IConsumerProducerOptions
{
    List<string> Servers { get; set; }

    string Group { get; set; }

    string Client { get; set; }
}