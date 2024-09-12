using RabbitEvents.Application.Interfaces.Events;

namespace RabbitEvents.Application.IntegrationEvents;

public abstract class WithImageEventBase : IIntegrationEvent
{
    protected WithImageEventBase(string fileExtension, string contentType)
    {
        FileExtension = fileExtension;
        ContentType = contentType;
    }

    public string FileExtension { get; private set; }
    public string ContentType { get; private set; }
}