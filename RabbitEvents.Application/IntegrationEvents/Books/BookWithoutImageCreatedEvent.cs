using RabbitEvents.Application.Interfaces.Events;

namespace RabbitEvents.Application.IntegrationEvents.Books;

public sealed class BookWithoutImageCreatedEvent : IIntegrationEvent
{
    public Guid BookId { get; private set; }
    public string BookTitle { get; private set; }
    public string LiteraryGenre { get; private set; }

    public BookWithoutImageCreatedEvent(Guid bookId, string bookTitle, string literaryGenre)
    {
        BookId = bookId;
        BookTitle = bookTitle;
        LiteraryGenre = literaryGenre;
    }
}
