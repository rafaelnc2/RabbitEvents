
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
        _queueService.CreateQueue(QueueDefinitions.AUTHORS_QUEUE);

        _queueService.CreateQueue(QueueDefinitions.IMAGES_QUEUE);

        return Task.CompletedTask;
    }
}
