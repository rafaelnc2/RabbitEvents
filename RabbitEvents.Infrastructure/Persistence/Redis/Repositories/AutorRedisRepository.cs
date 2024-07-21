namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

public class AutorRedisRepository
{
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<Autor> _autorCollection;
    private readonly IDatabase _database;
    private readonly ICacheService _cacheService;

    public AutorRedisRepository(IConnectionMultiplexer redisMultiplexerConnect, ICacheService cacheService)
    {
        _database = redisMultiplexerConnect.GetDatabase();
        _provider = new RedisConnectionProvider(redisMultiplexerConnect);
        _autorCollection = (RedisCollection<Autor>)_provider.RedisCollection<Autor>();
        _cacheService = cacheService;
    }

    public async Task<Autor> CriarAsync(Autor autor)
    {
        await _autorCollection.InsertAsync(autor);

        return autor;
    }
    public async Task<Autor> CriarAsync(Autor autor, byte[]? imageInBytes)
    {
        var taskList = new List<Task>();

        taskList.Add(_autorCollection.InsertAsync(autor));

        if (imageInBytes is not null)
        {
            var setImageToCacheTask = _cacheService.SetValueAsync($"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autor.Id}", imageInBytes, CacheKeysConstants.DEFAULT_EXPIRES);

            if (setImageToCacheTask is not null)
                taskList.Add(setImageToCacheTask);
        }

        await Task.WhenAll(taskList);

        return autor;
    }

    public async Task<Autor> AtualizarAsync(Autor autor)
    {
        await _autorCollection.UpdateAsync(autor);

        return autor;
    }

    public async Task<Autor?> ObterPorIdAsync(string autorId) =>
        await _autorCollection.FindByIdAsync(autorId);

    public async Task<IEnumerable<Autor>> ObterTodosAsync() =>
        await _autorCollection.ToListAsync();
}
