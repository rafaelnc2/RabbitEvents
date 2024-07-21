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

        taskList.Add(SetImageToCacheTask(autor.Id, imageInBytes));

        await Task.WhenAll(taskList);

        return autor;
    }

    public async Task<Autor> AtualizarAsync(Autor autor)
    {
        await _autorCollection.UpdateAsync(autor);

        return autor;
    }

    public async Task<Autor> AtualizarAsync(Autor autor, byte[]? imageInBytes)
    {
        var taskList = new List<Task>();

        taskList.Add(_autorCollection.UpdateAsync(autor));

        taskList.Add(SetImageToCacheTask(autor.Id, imageInBytes));

        await Task.WhenAll(taskList);

        return autor;
    }

    public async Task<Autor?> ObterPorIdAsync(string autorId) =>
        await _autorCollection.FindByIdAsync(autorId);

    public async Task<IEnumerable<Autor>> ObterTodosAsync() =>
        await _autorCollection.ToListAsync();


    private Task SetImageToCacheTask(Guid autorId, byte[]? imageInBytes)
    {
        if (imageInBytes is null)
            return Task.CompletedTask;

        return _cacheService.SetValueAsync($"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autorId}", imageInBytes, CacheKeysConstants.DEFAULT_EXPIRES)!;
    }
}
