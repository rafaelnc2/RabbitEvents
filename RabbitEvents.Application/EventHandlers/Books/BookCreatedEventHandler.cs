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

        _queueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.BOOKS_QUEUE,
            Exchange: null,
            RoutingKey: QueueDefinitions.BOOKS_QUEUE.RoutingKey,
            MessageBody: context.Message.Id.ToString()
        ));

        _queueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.LITERARY_GENRE_QUEUE,
            Exchange: null,
            RoutingKey: QueueDefinitions.LITERARY_GENRE_QUEUE.RoutingKey,
            MessageBody: context.Message.GeneroLiterario
        ));
    }
}
