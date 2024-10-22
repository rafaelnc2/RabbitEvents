using RabbitEvents.Application.IntegrationEvents.Books;
using RabbitEvents.Shared.Configurations;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Handlers.Books;

public sealed class BookWithoutImageCreatedEventHandler(
    ILogger<BookWithoutImageCreatedEventHandler> Logger,
    IQueueService QueueService
) : IConsumer<BookWithoutImageCreatedEvent>
{
    public async Task Consume(ConsumeContext<BookWithoutImageCreatedEvent> context)
    {
        Logger.LogInformation("Event handler BookWithoutImageUpdatedEventHandler");

        var descriptionForCreatingImage = $"Create a book cover of literary genre {context.Message.LiteraryGenre} with the following title {context.Message.BookTitle}";

        var messageBody = new BookImageCreateDto(
            BookId: context.Message.BookId,
            BookTitle: context.Message.BookTitle,
            LiteraryGenre: context.Message.LiteraryGenre,
            DescriptionForCreatingImage: descriptionForCreatingImage
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
