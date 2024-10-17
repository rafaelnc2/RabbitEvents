namespace RabbitEvents.Infrastructure.Persistence.Redis.Caching;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer redisMultiplexerConnect)
    {
        _database = redisMultiplexerConnect.GetDatabase();
    }

    public Task? SetValueAsync(string key, string value, int minutesToExpire = 15)
    {
        if (value is null)
            return null;

        TimeSpan expires = TimeSpan.FromMinutes(minutesToExpire);

        return _database.StringSetAsync(key, value, expires);
    }

    public Task? SetValueAsync(string key, byte[] value, int minutesToExpire = 15)
    {
        if (value is null)
            return null;

        TimeSpan expires = TimeSpan.FromMinutes(minutesToExpire);

        return _database.StringSetAsync(key, value, expires);
    }

    public async Task<string?> GetStringValueAsync(string key)
    {
        var cachedValue = await _database.StringGetAsync(key);

        return cachedValue.ToString();
    }

    public async Task<byte[]?> GetBytesValueAsync(string key)
    {
        var cachedValue = await _database.StringGetAsync(key);

        return cachedValue;
    }

    public Task DeleteValueAsync(string key)
    {
        return _database.StringGetDeleteAsync(key);
    }

    public Task<bool> KeyExistsAsync(string key) =>
        _database.KeyExistsAsync(key);


    public async Task<IEnumerable<string>> GetSetMembersAsync(string key)
    {
        if (_database.KeyExists(key) is false)
            return Enumerable.Empty<string>();

        var list = new List<string>();

        var members = await _database.SetMembersAsync(key);

        foreach (var member in members)
            list.Add(member.ToString());

        return list;
    }
}
