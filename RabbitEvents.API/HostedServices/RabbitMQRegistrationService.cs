
using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Constants;

namespace RabbitEvents.API.HostedServices;

public class RabbitMQRegistrationService : BackgroundService
{
    private readonly IQueueService _queueService;

    public RabbitMQRegistrationService(IQueueService queueService)
    {
        _queueService = queueService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _queueService.CreateMessageChannel(QueueDefinitions.AUTOR_ADD_QUEUE, QueueDefinitions.AUTOR_EXCHANGE);

        _queueService.CreateMessageChannel(QueueDefinitions.AUTOR_IMAGE_QUEUE, QueueDefinitions.AUTOR_EXCHANGE);

        return Task.CompletedTask;
    }
}
