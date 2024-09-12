using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.DomainEvents.BookEvents;

public class BookCreatedEvent : IDomainEvent
{
    public Guid Id { get; private set; }

    public BookCreatedEvent(Guid id)
    {
        Id = id;
    }
}
