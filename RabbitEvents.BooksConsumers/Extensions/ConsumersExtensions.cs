using RabbitEvents.BooksConsumers.Consumers;

namespace RabbitEvents.BooksConsumers.Extensions;

public static class ConsumersExtensions
{
    public static void AddConsumers(this IServiceCollection services)
    {
        services.AddHostedService<LiteraryGenreListUpdateConsumer>();
    }
}
