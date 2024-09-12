namespace RabbitEvents.Application.IntegrationEvents.Books;

public sealed class BookWithImageCreatedEvent : WithImageEventBase
{
    public Guid BookId { get; private set; }

    public BookWithImageCreatedEvent(Guid bookId, string fileExtension, string contentType) : base(fileExtension, contentType)
    {
        BookId = bookId;
    }
}
