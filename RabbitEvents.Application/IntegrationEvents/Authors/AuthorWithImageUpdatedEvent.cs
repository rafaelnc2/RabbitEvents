using RabbitEvents.Application.IntegrationEvents;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

public sealed class AuthorWithImageUpdatedEvent : WithImageEventBase
{
    public Guid AuthorId { get; private set; }

    public AuthorWithImageUpdatedEvent(Guid authorId, string fileExtension, string contentType) : base(fileExtension, contentType)
    {
        AuthorId = authorId;
    }
}
