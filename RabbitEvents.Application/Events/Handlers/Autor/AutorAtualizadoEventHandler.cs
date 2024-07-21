﻿using RabbitEvents.Domain.Events.AutorEvents;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.Events.Handlers.Autor;

public sealed class AutorAtualizadoEventHandler(
    ILogger<AutorCriadoEventHandler> Logger,
    ICacheService CacheService,
    IQueueService QueueService
) : IConsumer<AutorAtualizadoEvent>
{
    public async Task Consume(ConsumeContext<AutorAtualizadoEvent> context)
    {
        Logger.LogInformation("Criado Event handler AutorAtualizado");

        var autorId = context.Message.Id;
        var imageName = context.Message.ImageName;

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.AUTOR_QUEUE,
            Exchange: QueueDefinitions.AUTOR_EXCHANGE,
            RoutingKey: "autor.update.route",
            MessageBody: autorId.ToString()
        ));

        var messageBody = $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autorId}";

        var keyExists = await CacheService.KeyExistsAsync(messageBody);

        if (keyExists is true && string.IsNullOrEmpty(imageName) is false)
        {
            QueueService.SendMessage(new QueueMessage(
                Queue: QueueDefinitions.AUTOR_IMAGE_QUEUE,
                Exchange: QueueDefinitions.AUTOR_EXCHANGE,
                RoutingKey: "autor.image.route",
                MessageBody: messageBody
            ));
        }
    }
}
