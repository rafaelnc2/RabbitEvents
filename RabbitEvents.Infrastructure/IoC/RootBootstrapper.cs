namespace RabbitEvents.Infrastructure.IoC;

public class RootBootstrapper
{
    public void BootstrapperRegisterServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConfiguration>(config);

        new RedisBootstrapper().Register(services, config);

        new MassTransitBoostrapper().Register(services);

        new ServicesBootstrapper().ServicesRegister(services, config);

        new RepositoriesBootstrapper().RepositoriesRegister(services);
    }
}
