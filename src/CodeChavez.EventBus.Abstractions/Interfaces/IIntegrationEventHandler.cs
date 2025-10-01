using CodeChavez.EventBus.Abstractions.EventNotifications;

namespace CodeChavez.EventBus.Abstractions.Interfaces;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler { }

