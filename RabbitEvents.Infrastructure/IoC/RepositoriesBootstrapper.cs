using Microsoft.Extensions.DependencyInjection;
using RabbitEvents.Domain.Interfaces.Repositories;
using RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

namespace RabbitEvents.Infrastructure.IoC;

public class RepositoriesBootstrapper
{
    public void RepositoriesRegister(IServiceCollection services)
    {
        services.AddTransient<AuthorRedisRepository>();
        services.AddScoped<IAutorRedisRepository, AuthorRedisDecoratedRepository>();
    }
}
