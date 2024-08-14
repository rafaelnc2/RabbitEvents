using RabbitEvents.Domain.Events.AutorEvents;
using RabbitEvents.Shared.Models.Messaging;

namespace RabbitEvents.Application.Events.Handlers.Autor;

public sealed class AutorCriadoEventHandler : IConsumer<AutorCriadoEvent>
{
    private readonly ILogger<AutorCriadoEventHandler> _logger;
    private readonly IQueueService _queueService;

    public AutorCriadoEventHandler(ILogger<AutorCriadoEventHandler> logger, IQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    public async Task Consume(ConsumeContext<AutorCriadoEvent> context)
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
