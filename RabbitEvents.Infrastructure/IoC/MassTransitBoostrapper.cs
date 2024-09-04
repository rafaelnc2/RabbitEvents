using RabbitEvents.Application.EventHandlers.Authors;
using RabbitEvents.Application.EventHandlers.Books;
using RabbitEvents.Infrastructure.IntegrationEvents.Handlers.Authors;
using RabbitEvents.Infrastructure.Shared;

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

            x.AddConsumer<BookCreatedEventHandler>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddTransient<EventSender>();
    }
}
