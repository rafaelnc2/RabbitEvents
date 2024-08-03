using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.Events.AutorEvents;

public class AutorCriadoEvent : IDomainEvent
{
    public Guid Id { get; private set; }
    public string FileExtension { get; private set; }
    public string ContentType { get; private set; }

    public AutorCriadoEvent(Guid id, string fileExtension, string contentType)
    {
        Id = id;
        FileExtension = fileExtension;
        ContentType = contentType;
    }
}