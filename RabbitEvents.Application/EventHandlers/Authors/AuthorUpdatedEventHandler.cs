using RabbitEvents.Domain.DomainEvents.AuthorEvents;
using RabbitEvents.Shared.Configurations;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.EventHandlers.Authors;

public sealed class AuthorUpdatedEventHandler(
    ILogger<AuthorCreatedEventHandler> Logger,
    IQueueService QueueService
) : IConsumer<AuthorUpdatedEvent>
{
    public Task Consume(ConsumeContext<AuthorUpdatedEvent> context)
    {
        Logger.LogInformation("Criado Event handler AutorAtualizado");

        var authorId = context.Message.Id;

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.AUTHORS_QUEUE,
            Exchange: null,
            RoutingKey: QueueDefinitions.AUTHORS_QUEUE.RoutingKey,
            MessageBody: authorId.ToString()
        ));

        return Task.CompletedTask;
    }
}
