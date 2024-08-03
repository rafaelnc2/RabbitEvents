using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitEvents.Infrastructure.Persistence.Redis.Caching;

namespace RabbitEvents.Infrastructure.IoC;

public class RedisBootstrapper
{
    public void Register(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Redis")!;

        services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(connectionString));

        services.AddSingleton<ICacheService, CacheService>();
    }
}
