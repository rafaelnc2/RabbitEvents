using RabbitEvents.Application.EventHandlers.Authors;
using RabbitEvents.Infrastructure.IntegrationEvents.Handlers.Authors;

namespace RabbitEvents.Infrastructure.IoC;

internal class MassTransitBoostrapper
{
    public void Register(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AuthorCreatedEventHandler>();
            x.AddConsumer<AuthorUpdatedEventHandler>();

            x.AddConsumer<AuthorWithImageCreatedEventHandler>();
            x.AddConsumer<AuthorWithoutImageCreatedEventHandler>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
