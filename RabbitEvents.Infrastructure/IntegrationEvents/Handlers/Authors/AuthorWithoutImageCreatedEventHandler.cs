﻿using RabbitEvents.Domain.IntegrationEvents.AutorEvents;
using RabbitEvents.Shared.Configurations;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Handlers.Authors;

public sealed class AuthorWithoutImageCreatedEventHandler(
    ILogger<AuthorWithImageCreatedEventHandler> Logger,
    IQueueService QueueService
) : IConsumer<AuthorWithoutImageCreatedEvent>
{
    public async Task Consume(ConsumeContext<AuthorWithoutImageCreatedEvent> context)
    {
        Logger.LogInformation("Event handler AuthorWithoutImageCreatedEventHandler");

        var messageBody = new AuthorImageCreateDto(
            AuthorId: context.Message.AuthorId,
            AuthorName: context.Message.AuthorName
        );

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.IMAGES_CREATE_QUEUE,
            Exchange: QueueDefinitions.IMAGES_EXCHANGE,
            RoutingKey: QueueDefinitions.IMAGES_CREATE_QUEUE.RoutingKey,
            MessageBody: JsonSerializer.Serialize(messageBody)
        ));

        await Task.CompletedTask;
    }
}
