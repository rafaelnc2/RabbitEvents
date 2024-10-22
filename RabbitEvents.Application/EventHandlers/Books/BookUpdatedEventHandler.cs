using RabbitEvents.Domain.DomainEvents.BookEvents;
using RabbitEvents.Shared.Configurations;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.EventHandlers.Books;

public sealed class BookUpdatedEventHandler(
    ILogger<BookUpdatedEventHandler> Logger,
    IQueueService QueueService
) : IConsumer<BookUpdatedEvent>
{
    public Task Consume(ConsumeContext<BookUpdatedEvent> context)
    {
        Logger.LogInformation("Criado Event handler LivroAtualizado");

        var bookId = context.Message.Id;

        QueueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.BOOKS_QUEUE,
            Exchange: null,
            RoutingKey: QueueDefinitions.BOOKS_QUEUE.RoutingKey,
            MessageBody: bookId.ToString()
        ));

        return Task.CompletedTask;
    }
}
