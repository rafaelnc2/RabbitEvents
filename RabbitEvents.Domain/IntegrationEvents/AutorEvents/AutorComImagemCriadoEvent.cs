using RabbitEvents.Domain.Interfaces.Events;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

public sealed class AutorComImagemCriadoEvent : IIntegrationEvent
{
    public Guid AutorId { get; private set; }
    public string FileExtension { get; private set; }
    public string ContentType { get; private set; }

    public AutorComImagemCriadoEvent(Guid autorId, string fileExtension, string contentType)
    {
        AutorId = autorId;
        FileExtension = fileExtension;
        ContentType = contentType;
    }
}
