namespace CodeChavez.EventBus.Abstractions.Interfaces;

public interface IEventBusProducer
{
    Task PublishAsync<TEvent>(TEvent @event, string? topic = null, CancellationToken cancellationToken = default)
        where TEvent : class;
}
