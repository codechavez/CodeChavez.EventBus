using Azure.Messaging.EventHubs;

namespace CodeChavez.EventBus.EventHub.Interfaces;

public interface IEventBusConnection
{
    Task<EventProcessorClient> GetProcessorClientAsync();
}