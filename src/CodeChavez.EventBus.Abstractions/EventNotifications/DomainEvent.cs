using MediatR;

namespace CodeChavez.EventBus.Abstractions.EventNotifications;

public abstract class DomainEvent : INotification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
}