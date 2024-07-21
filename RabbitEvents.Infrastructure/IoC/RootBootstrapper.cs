using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitEvents.Infrastructure.IoC;

public class RootBootstrapper
{
    public void BootstrapperRegisterServices(IServiceCollection services, IConfiguration config)
    {
        new RedisBootstrapper().Register(services, config);

        new MassTransitBoostrapper().Register(services);

        new ServicesBootstrapper().ServicesRegister(services);

        new RepositoriesBootstrapper().RepositoriesRegister(services);
    }
}
