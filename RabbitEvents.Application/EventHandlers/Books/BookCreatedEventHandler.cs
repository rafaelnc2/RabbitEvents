using RabbitEvents.Domain.DomainEvents.BookEvents;

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
    }
}
