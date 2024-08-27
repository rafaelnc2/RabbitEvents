using RabbitEvents.Domain.DomainEvents.AuthorEvents;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.EventHandlers.Authors;

public sealed class AuthorCreatedEventHandler : IConsumer<AuthorCreatedEvent>
{
    private readonly ILogger<AuthorCreatedEventHandler> _logger;
    private readonly IQueueService _queueService;

    public AuthorCreatedEventHandler(ILogger<AuthorCreatedEventHandler> logger, IQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    public async Task Consume(ConsumeContext<AuthorCreatedEvent> context)
    {
        _logger.LogInformation("Event handler AutorCriado");

        var autorId = context.Message.Id;

        _queueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.AUTHORS_QUEUE,
            Exchange: null,
            RoutingKey: "authors",
            MessageBody: autorId.ToString()
        ));
    }
}
