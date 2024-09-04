using RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

namespace RabbitEvents.Infrastructure.IoC;

public class RepositoriesBootstrapper
{
    public void RepositoriesRegister(IServiceCollection services)
    {
        services.AddTransient<AuthorRedisRepository>();
        services.AddScoped<IAuthorRedisRepository, AuthorRedisDecoratedRepository>();

        services.AddScoped<BookRedisRepository>();
        services.AddScoped<IBookRedisRepository, BookRedisDecoratedRepository>();
    }
}
