namespace SharedKernel.Messaging.Events;

public class IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccuredAt { get; } = DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName!;
}