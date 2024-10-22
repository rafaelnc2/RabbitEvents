using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.DomainEvents.BookEvents;

public sealed class BookUpdatedEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public BookUpdatedEvent(Guid id)
    {
        Id = id;
    }
}
