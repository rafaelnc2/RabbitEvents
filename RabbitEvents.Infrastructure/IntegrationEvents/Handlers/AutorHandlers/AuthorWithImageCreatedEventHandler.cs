using RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Handlers.AutorHandlers;

public sealed class AuthorWithImageCreatedEventHandler(
    ILogger<AuthorWithImageCreatedEventHandler> Logger,
    ICacheService CacheService,
    IQueueService QueueService
) : IConsumer<AuthorWithImageCreatedEvent>
{
    public async Task Consume(ConsumeContext<AuthorWithImageCreatedEvent> context)
    {
        Logger.LogInformation("Event handler AuthorWithImageCreatedEventHandler");

        var authorIdCacheKey = $"{CacheKeysConstants.AUTHOR_IMAGE_KEY}:{context.Message.AuthorId}";

        var keyExists = await CacheService.KeyExistsAsync(authorIdCacheKey);

        if (keyExists)
        {
            var messageBody = new ImageMessageBodyDto(
                ImageId: authorIdCacheKey,
                FileExtension: context.Message.FileExtension,
                ContentType: context.Message.ContentType
            );

            QueueService.SendMessage(new QueueMessage(
                Queue: QueueDefinitions.IMAGES_UPLOAD_QUEUE,
                Exchange: QueueDefinitions.IMAGES_EXCHANGE,
                RoutingKey: QueueDefinitions.IMAGES_UPLOAD_QUEUE.RoutingKey,
                MessageBody: JsonSerializer.Serialize(messageBody)
            ));

            return;
        }

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.IMAGES_CREATE_QUEUE,
            Exchange: QueueDefinitions.IMAGES_EXCHANGE,
            RoutingKey: QueueDefinitions.IMAGES_CREATE_QUEUE.RoutingKey,
            MessageBody: authorIdCacheKey
        ));
    }
}
