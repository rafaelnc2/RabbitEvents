using RabbitEvents.Domain.DomainEvents.BookEvents;
using RabbitEvents.Shared.Configurations;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.EventHandlers.Books;

public sealed class BookCreatedEventHandler : IConsumer<BookCreatedEvent>
{
    private readonly ILogger<BookCreatedEventHandler> _logger;
    private readonly IQueueService _queueService;

    public BookCreatedEventHandler(ILogger<BookCreatedEventHandler> logger, IQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    public async Task Consume(ConsumeContext<BookCreatedEvent> context)
    {
        _logger.LogInformation("Event handler LivroCriado");

        var bookId = context.Message.Id;

        _queueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.BOOKS_QUEUE,
            Exchange: null,
            RoutingKey: "books",
            MessageBody: bookId.ToString()
        ));
    }
}
