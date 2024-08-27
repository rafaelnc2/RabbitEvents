using RabbitEvents.Application.Interfaces.Events;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

public sealed class AuthorWithImageCreatedEvent : IIntegrationEvent
{
    public Guid AuthorId { get; private set; }
    public string FileExtension { get; private set; }
    public string ContentType { get; private set; }

    public AuthorWithImageCreatedEvent(Guid authorId, string fileExtension, string contentType)
    {
        AuthorId = authorId;
        FileExtension = fileExtension;
        ContentType = contentType;
    }
}
