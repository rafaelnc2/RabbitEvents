namespace RabbitEvents.Domain.Interfaces.Events;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent);
}
