﻿using RabbitEvents.ImagesConsumers.Consumers;

namespace RabbitEvents.ImagesConsumers.Extensions;

public static class ConsumersExtensions
{
    public static void AddConsumers(this IServiceCollection services)
    {
        services.AddHostedService<ImageUploadConsumer>();
        services.AddHostedService<AuthorImageUpdateConsumer>();
        services.AddHostedService<ImageCreateConsumer>();
        services.AddHostedService<BookImageUpdateConsumer>();
    }
}
