using CodeChavez.EventBus.EventHub.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChavez.EventBus.EventHub;

public static class EventHubModule
{
    public static IServiceCollection AddEventHubConnection(this IServiceCollection services)
    {
        services.AddSingleton<IEventBusConnection, EventHubConnection>();

        return services;
    }
}
