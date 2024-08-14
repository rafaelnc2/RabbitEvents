
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
        //_queueService.CreateQueue(QueueDefinitions.AUTHORS_QUEUE);
        _queueService.CreateTopic(QueueDefinitions.AUTHORS_QUEUE, QueueDefinitions.AUTHORS_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.AUTHORS_IMAGE_UPDATE_QUEUE, QueueDefinitions.AUTHORS_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.IMAGES_UPLOAD_QUEUE, QueueDefinitions.IMAGES_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.IMAGES_CREATE_QUEUE, QueueDefinitions.IMAGES_EXCHANGE);

        return Task.CompletedTask;
    }
}
