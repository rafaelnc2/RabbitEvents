using RabbitEvents.Domain.Events.AutorEvents;
using RabbitEvents.Shared.Models.Messaging;

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

        var messageBody = $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autorId}";

        var keyExists = await CacheService.KeyExistsAsync(messageBody);

        if (keyExists)
        {
            QueueService.SendMessage(new QueueMessage(
                Queue: QueueDefinitions.IMAGES_QUEUE,
                Exchange: null,
                RoutingKey: "images",
                MessageBody: messageBody
            ));
        }
    }
}
