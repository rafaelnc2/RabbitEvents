using RabbitEvents.Application.IntegrationEvents.Books;
using RabbitEvents.Shared.Configurations;

namespace RabbitEvents.Infrastructure.IntegrationEvents.Handlers.Books;

public sealed class BookWithImageCreatedEventHandler(
    ILogger<BookWithImageCreatedEventHandler> Logger,
    ICacheService CacheService,
    IQueueService QueueService
) : IConsumer<BookWithImageCreatedEvent>
{
    public async Task Consume(ConsumeContext<BookWithImageCreatedEvent> context)
    {
        Logger.LogInformation("Event handler BookWithImageCreatedEventHandler");

        var bookIdCacheKey = $"{CacheKeysConstants.BOOK_IMAGE_KEY}:{context.Message.BookId}";

        var keyExists = await CacheService.KeyExistsAsync(bookIdCacheKey);

        if (keyExists)
        {
            var messageBody = new ImageMessageBodyDto(
                ImageId: bookIdCacheKey,
                FileExtension: context.Message.FileExtension,
                ContentType: context.Message.ContentType,
                BlobContainerName: BlobStorageConstants.BooksImageContainerName,
                DestinationQueue: QueueDefinitions.BOOKS_IMAGE_UPDATE_QUEUE,
                DestinationExchange: QueueDefinitions.BOOKS_EXCHANGE
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
            MessageBody: bookIdCacheKey
        ));
    }
}
