using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.Events.AutorEvents;
public sealed class AuthorUpdatedEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public AuthorUpdatedEvent(Guid id)
    {
        Id = id;
    }
}
