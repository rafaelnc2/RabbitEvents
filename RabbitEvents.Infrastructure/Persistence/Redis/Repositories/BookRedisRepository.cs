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
