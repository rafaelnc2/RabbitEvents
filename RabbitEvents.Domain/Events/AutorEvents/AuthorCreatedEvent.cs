using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.Events.AutorEvents;

public class AuthorCreatedEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public AuthorCreatedEvent(Guid id)
    {
        Id = id;
    }
}