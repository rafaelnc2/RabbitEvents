using RabbitEvents.Infrastructure.Shared;

namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

internal class BookRedisDecoratedRepository : IBookRedisRepository
{
    private readonly BookRedisRepository _bookRedisRepository;
    private readonly EventSender _eventSender;

    public BookRedisDecoratedRepository(BookRedisRepository bookRedisRepository, EventSender eventSender)
    {
        _bookRedisRepository = bookRedisRepository;
        _eventSender = eventSender;
    }

    public async Task<Book> CriarAsync(Book book)
    {
        await _bookRedisRepository.CriarAsync(book);

        await _eventSender.PublishEventsAsync(book);

        return book;
    }

    public Task<Book> AtualizarAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book?> ObterPorIdAsync(string bookId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> ObterTodosAsync(string? filterName)
    {
        throw new NotImplementedException();
    }
}
