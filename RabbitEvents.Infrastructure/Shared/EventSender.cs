namespace RabbitEvents.Infrastructure.Shared;

internal class EventSender
{
    private readonly IBus _bus;

    public EventSender(IBus bus)
    {
        _bus = bus;
    }

    public async ValueTask PublishEventsAsync(Entity entity)
    {
        if (entity.DomainEvents.Any() is false)
            return;

        foreach (IDomainEvent @event in entity.DomainEvents)
        {
            await _bus.Publish((object)@event);
        }

        entity.ClearEvents();
    }
}
