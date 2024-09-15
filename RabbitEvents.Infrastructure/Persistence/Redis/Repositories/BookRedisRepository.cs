namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

public class BookRedisRepository : IBookRedisRepository
{
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<Book> _bookCollection;
    private readonly IDatabase _database;
    private readonly ICacheService _cacheService;

    public BookRedisRepository(IConnectionMultiplexer redisMultiplexerConnect, ICacheService cacheService)
    {
        _database = redisMultiplexerConnect.GetDatabase();
        _provider = new RedisConnectionProvider(redisMultiplexerConnect);
        _bookCollection = (RedisCollection<Book>)_provider.RedisCollection<Book>();
        _cacheService = cacheService;
    }

    public async Task<Book> CriarAsync(Book book)
    {
        await _bookCollection.InsertAsync(book);

        return book;
    }

    public async Task<Book> AtualizarAsync(Book book)
    {
        await _bookCollection.UpdateAsync(book);

        return book;
    }

    public Task<Book?> ObterPorIdAsync(string bookId) =>
        _bookCollection.FindByIdAsync(bookId);

    public async Task<IEnumerable<Book>> ObterTodosAsync(string? filterTitle)
    {
        if (string.IsNullOrWhiteSpace(filterTitle) is true)
            return await _bookCollection.ToListAsync();

        return await _bookCollection.Where(book => book.Titulo.Contains(filterTitle)).ToListAsync();
    }
}
