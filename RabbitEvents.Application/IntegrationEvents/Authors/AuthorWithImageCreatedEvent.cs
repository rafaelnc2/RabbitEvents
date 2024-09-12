using RabbitEvents.Application.IntegrationEvents;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

public sealed class AuthorWithImageCreatedEvent : WithImageEventBase
{
    public Guid AuthorId { get; private set; }

    public AuthorWithImageCreatedEvent(Guid authorId, string fileExtension, string contentType) : base(fileExtension, contentType)
    {
        AuthorId = authorId;
    }
}
