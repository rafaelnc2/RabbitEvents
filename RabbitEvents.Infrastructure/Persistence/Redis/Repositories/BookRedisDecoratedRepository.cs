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

    public async Task<Book> AtualizarAsync(Book book)
    {
        await _bookRedisRepository.AtualizarAsync(book);

        await _eventSender.PublishEventsAsync(book);

        return book;
    }

    public Task<Book?> ObterPorIdAsync(string bookId) =>
        _bookRedisRepository.ObterPorIdAsync(bookId);

    public Task<IEnumerable<Book>> ObterTodosAsync(string? filterTitle) =>
        _bookRedisRepository.ObterTodosAsync(filterTitle);
}
