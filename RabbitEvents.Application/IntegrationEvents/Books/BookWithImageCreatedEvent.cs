using RabbitEvents.Application.Interfaces.Events;

namespace RabbitEvents.Application.IntegrationEvents.Books;

public sealed class BookWithImageCreatedEvent : IIntegrationEvent
{
    public Guid BookId { get; private set; }
    public string FileExtension { get; private set; }
    public string ContentType { get; private set; }

    public BookWithImageCreatedEvent(Guid bookId, string fileExtension, string contentType)
    {
        BookId = bookId;
        FileExtension = fileExtension;
        ContentType = contentType;
    }
}
