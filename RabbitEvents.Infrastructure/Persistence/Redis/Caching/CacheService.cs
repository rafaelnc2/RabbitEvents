using RabbitEvents.Application.Interfaces;
using StackExchange.Redis;

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

    public Task GetValueAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task DeleteValueAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> KeyExistsAsync(string key) =>
        _database.KeyExistsAsync(key);
}
