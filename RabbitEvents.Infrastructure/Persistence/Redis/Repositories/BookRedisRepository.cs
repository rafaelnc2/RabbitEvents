using RabbitEvents.Shared.Inputs.Books;

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

    public IEnumerable<Book> ObterTodos(GetBooksByFiltersInput filtersInput)
    {
        var query = _bookCollection.AsQueryable();

        if (string.IsNullOrWhiteSpace(filtersInput.Titulo) is false)
            query = query.Where(b => b.Titulo.StartsWith(filtersInput.Titulo));

        if (string.IsNullOrWhiteSpace(filtersInput.Editora) is false)
            query = query.Where(b => b.Editora.StartsWith(filtersInput.Editora));

        if (string.IsNullOrWhiteSpace(filtersInput.GeneroLiterario) is false)
            query = query.Where(b => b.GeneroLiterario.Contains(filtersInput.GeneroLiterario));

        if (string.IsNullOrWhiteSpace(filtersInput.IdAutor) is false)
            query = query.Where(b => b.AuthorInfo.Id == filtersInput.IdAutor);

        if (string.IsNullOrWhiteSpace(filtersInput.NomeAutor) is false)
            query = query.Where(b => b.AuthorInfo.Nome.StartsWith(filtersInput.NomeAutor));

        return query.ToList();
    }
}
