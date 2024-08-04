using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.DomainEvents.AutorEvents;

public class AutorCriadoEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public AutorCriadoEvent(Guid id)
    {
        Id = id;
    }
}