using RabbitEvents.Infrastructure.IoC;

namespace RabbitEvents.API.Extensions;

public static class ServicesBootstrapperExtension
{
    public static void AddBootstrapperRegister(this IServiceCollection services, IConfiguration config)
    {
        new RootBootstrapper().BootstrapperRegisterServices(services, config);
    }
}
