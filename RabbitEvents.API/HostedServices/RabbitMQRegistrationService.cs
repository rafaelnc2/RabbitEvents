
using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Configurations;

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


        _queueService.CreateTopic(QueueDefinitions.BOOKS_QUEUE, QueueDefinitions.BOOKS_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.BOOKS_IMAGE_UPDATE_QUEUE, QueueDefinitions.BOOKS_EXCHANGE);


        _queueService.CreateQueue(QueueDefinitions.AUTHORS_QUEUE);


        _queueService.CreateQueue(QueueDefinitions.LITERARY_GENRE_QUEUE);

        _queueService.CreateDeadLetterQueue(QueueDefinitions.IMAGES_DLQ_QUEUE, QueueDefinitions.IMAGES_DLQ_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.IMAGES_UPLOAD_QUEUE, QueueDefinitions.IMAGES_EXCHANGE);

        _queueService.CreateTopic(QueueDefinitions.IMAGES_CREATE_QUEUE, QueueDefinitions.IMAGES_EXCHANGE);

        return Task.CompletedTask;
    }
}
