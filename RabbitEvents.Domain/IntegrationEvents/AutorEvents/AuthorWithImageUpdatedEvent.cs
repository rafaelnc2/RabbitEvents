using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

public sealed record AuthorWithImageUpdatedEvent(Guid AuthorId, string FileExtension, string ContentType) : IIntegrationEvent;
