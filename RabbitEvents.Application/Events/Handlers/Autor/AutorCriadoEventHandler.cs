using RabbitEvents.Domain.Events.AutorEvents;
using RabbitEvents.Shared.Dtos;
using RabbitEvents.Shared.Models.Messaging;
using System.Text.Json;

namespace RabbitEvents.Application.Events.Handlers.Autor;

public sealed class AutorCriadoEventHandler(
    ILogger<AutorCriadoEventHandler> Logger,
    ICacheService CacheService,
    IQueueService QueueService
) : IConsumer<AutorCriadoEvent>
{
    public async Task Consume(ConsumeContext<AutorCriadoEvent> context)
    {
        Logger.LogInformation("Event handler AutorCriado");

        var autorId = context.Message.Id;

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.AUTHORS_QUEUE,
            Exchange: null,
            RoutingKey: "authors",
            MessageBody: autorId.ToString()
        ));

        var authorIdCacheKey = $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autorId}";

        var keyExists = await CacheService.KeyExistsAsync(authorIdCacheKey);

        if (keyExists)
        {
            var messageBody = new ImageMessageBodyDto(
                ImageId: autorId.ToString(),
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
