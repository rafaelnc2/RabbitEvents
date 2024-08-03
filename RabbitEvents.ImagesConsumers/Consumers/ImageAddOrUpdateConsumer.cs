using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Constants;

namespace RabbitEvents.ImagesConsumers.Consumers;

public sealed class ImageAddOrUpdateConsumer : BackgroundService
{
    private readonly ILogger<ImageAddOrUpdateConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICacheService _cacheService;

    public ImageAddOrUpdateConsumer(ILogger<ImageAddOrUpdateConsumer> logger,
        IQueueService queueService,
        IServiceProvider serviceProvider,
        ICacheService cacheService)
    {
        _logger = logger;
        _queueService = queueService;
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(ImageAddOrUpdateConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.IMAGES_ADD_UPDATE_QUEUE.Name,
            messageHandlerAsync: HandleMessageConsumerAsync,
            cancellationToken: stoppingToken
        );
    }

    private async Task HandleMessageConsumerAsync(string message, ulong deliveryTag)
    {
        _logger.LogInformation($"Processando a mensagem: {message}");

        var autorImageString = await _cacheService.GetValueAsync(message);

        if (string.IsNullOrWhiteSpace(autorImageString))
        {
            _logger.LogWarning($"A imagem {message} não existe no cache");
            return;
        }

        byte[] autorImageBytes = System.Text.Encoding.UTF8.GetBytes(autorImageString);

        Stream stream = new MemoryStream(autorImageBytes);

        //var autorid = message.Split(AutorImagePrefixSeparator).LastOrDefault();

        //using var scope = _serviceProvider.CreateScope();

        //IAutorRedisRepository autorRedisRepository = scope.ServiceProvider.GetService<IAutorRedisRepository>()!;
    }
}
