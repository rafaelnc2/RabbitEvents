using RabbitEvents.Application.Events.Handlers.Autor;
using RabbitEvents.Infrastructure.IntegrationEvents.Handlers.AutorHandlers;

namespace RabbitEvents.Infrastructure.IoC;

internal class MassTransitBoostrapper
{
    public void Register(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AutorCriadoEventHandler>();
            x.AddConsumer<AutorAtualizadoEventHandler>();

            x.AddConsumer<AutorComImagemCriadoEventHandler>();
            x.AddConsumer<AuthorWithoutImageCreatedEventHandler>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
