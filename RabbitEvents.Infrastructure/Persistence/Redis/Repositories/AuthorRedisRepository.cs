namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

public class AuthorRedisRepository
{
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<Author> _autorCollection;
    private readonly IDatabase _database;
    private readonly ICacheService _cacheService;

    public AuthorRedisRepository(IConnectionMultiplexer redisMultiplexerConnect, ICacheService cacheService)
    {
        _database = redisMultiplexerConnect.GetDatabase();
        _provider = new RedisConnectionProvider(redisMultiplexerConnect);
        _autorCollection = (RedisCollection<Author>)_provider.RedisCollection<Author>();
        _cacheService = cacheService;
    }

    public async Task<Author> CriarAsync(Author autor)
    {
        await _autorCollection.InsertAsync(autor);

        return autor;
    }
    public async Task<Author> CriarAsync(Author autor, byte[]? imageInBytes)
    {
        var taskList = new List<Task>();

        taskList.Add(_autorCollection.InsertAsync(autor));

        taskList.Add(SetImageToCacheTask(autor.Id, imageInBytes));

        await Task.WhenAll(taskList);

        return autor;
    }

    public async Task<Author> AtualizarAsync(Author autor)
    {
        await _autorCollection.UpdateAsync(autor);

        return autor;
    }

    public async Task<Author> AtualizarAsync(Author autor, byte[]? imageInBytes)
    {
        var taskList = new List<Task>();

        taskList.Add(_autorCollection.UpdateAsync(autor));

        taskList.Add(SetImageToCacheTask(autor.Id, imageInBytes));

        await Task.WhenAll(taskList);

        return autor;
    }

    public async Task<Author?> ObterPorIdAsync(string autorId) =>
        await _autorCollection.FindByIdAsync(autorId);

    public async Task<IEnumerable<Author>> ObterTodosAsync(string? filterName)
    {
        if (string.IsNullOrWhiteSpace(filterName) is true)
            return await _autorCollection.ToListAsync();

        return await _autorCollection.Where(author => author.Nome.Contains(filterName)).ToListAsync();
    }


    private Task SetImageToCacheTask(Guid autorId, byte[]? imageInBytes)
    {
        if (imageInBytes is null)
            return Task.CompletedTask;

        return _cacheService.SetValueAsync($"{CacheKeysConstants.AUTHOR_IMAGE_KEY}:{autorId}", imageInBytes, CacheKeysConstants.DEFAULT_EXPIRES) ?? Task.CompletedTask;
    }
}
