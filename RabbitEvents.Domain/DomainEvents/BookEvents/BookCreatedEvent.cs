using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.DomainEvents.BookEvents;

public sealed class BookCreatedEvent : IDomainEvent
{
    public Guid Id { get; private set; }
    public string GeneroLiterario { get; private set; }

    public BookCreatedEvent(Guid id, string generoLiterario)
    {
        Id = id;
        GeneroLiterario = generoLiterario;
    }
}
