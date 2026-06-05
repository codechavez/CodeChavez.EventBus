using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace CodeChavez.EventBus.EventHub.Interfaces;

public interface IEventBusConnection
{
    Task<EventProcessorClient> GetConsumerClientAsync();
    Task<EventHubProducerClient> GetProducerClientAsync();
}