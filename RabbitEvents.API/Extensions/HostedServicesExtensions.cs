using RabbitEvents.API.HostedServices;

namespace RabbitEvents.API.Extensions;

public static class HostedServicesExtensions
{
    public static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<RedisIndexCreationService>();
        services.AddHostedService<RabbitMQRegistrationService>();
    }
}
