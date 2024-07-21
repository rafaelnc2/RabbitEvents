using Microsoft.Extensions.DependencyInjection;
using RabbitEvents.Application.Services;
using RabbitEvents.Domain.Interfaces.DomainServices;
using RabbitEvents.Infrastructure.Messaging;

namespace RabbitEvents.Infrastructure.IoC;

internal class ServicesBootstrapper
{
    public void ServicesRegister(IServiceCollection services)
    {
        services.AddSingleton<IQueueService, QueueService>();
        services.AddScoped<IAutorDomainService, AutorService>();
    }
}
