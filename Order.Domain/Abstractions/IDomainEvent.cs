using MediatR;

namespace Order.Domain.Abstractions;
#pragma warning disable CS8603
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccuredAt => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}