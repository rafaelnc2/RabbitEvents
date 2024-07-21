using Microsoft.Extensions.DependencyInjection;
using RabbitEvents.Application.Events.Handlers.Autor;

namespace RabbitEvents.Infrastructure.IoC;

internal class MassTransitBoostrapper
{
    public void Register(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AutorCriadoEventHandler>();
            x.AddConsumer<AutorAtualizadoEventHandler>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
