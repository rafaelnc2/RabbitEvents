using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.Events.AutorEvents;
public sealed class AutorAtualizadoEvent : IDomainEvent
{
    public Guid Id { get; private set; }
    public string ImageName { get; private set; }

    public AutorAtualizadoEvent(Guid id, string imageName)
    {
        Id = id;
        ImageName = imageName;
    }
}
