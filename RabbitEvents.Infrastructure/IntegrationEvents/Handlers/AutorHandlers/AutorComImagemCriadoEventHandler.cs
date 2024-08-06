using RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Handlers.AutorHandlers;

public sealed class AutorComImagemCriadoEventHandler(
    ILogger<AutorComImagemCriadoEventHandler> Logger,
    ICacheService CacheService,
    IQueueService QueueService
) : IConsumer<AutorComImagemCriadoEvent>
{
    public async Task Consume(ConsumeContext<AutorComImagemCriadoEvent> context)
    {
        Logger.LogInformation("Event handler AutorComImagemCriadoEventHandler");

        var authorIdCacheKey = $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{context.Message.AutorId}";

        var keyExists = await CacheService.KeyExistsAsync(authorIdCacheKey);

        if (keyExists)
        {
            var messageBody = new ImageMessageBodyDto(
                ImageId: authorIdCacheKey,
                FileExtension: context.Message.FileExtension,
                ContentType: context.Message.ContentType
            );

            QueueService.SendMessage(new QueueMessage(
                Queue: QueueDefinitions.IMAGES_ADD_UPDATE_QUEUE,
                Exchange: QueueDefinitions.IMAGES_EXCHANGE,
                RoutingKey: QueueDefinitions.IMAGES_ADD_UPDATE_QUEUE.RoutingKey,
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
