using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Domain.IntegrationEvents.AutorEvents;

public sealed class AuthorWithoutImageCreatedEvent : IIntegrationEvent
{
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; private set; }

    public AuthorWithoutImageCreatedEvent(Guid authorId, string authorName)
    {
        AuthorId = authorId;
        AuthorName = authorName;
    }
}
