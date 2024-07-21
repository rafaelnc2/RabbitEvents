using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.Events.AutorEvents;

public class AutorCriadoEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public AutorCriadoEvent(Guid id)
    {
        Id = id;
    }
}