namespace RabbitEvents.ImagesConsumers.Consumers;

public sealed class ImageCreateConsumer : BackgroundService
{
    private readonly ILogger<ImageUploadConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly ICacheService _cacheService;
    private readonly string _apiKey;

    public ImageCreateConsumer(IConfiguration configuration, ILogger<ImageUploadConsumer> logger, IQueueService queueService, ICacheService cacheService)
    {
        _apiKey = configuration["OpenIaAccess:ApiKey"]!;

        _logger = logger;
        _queueService = queueService;
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(ImageCreateConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.IMAGES_CREATE_QUEUE.Name,
            messageHandlerAsync: HandleMessageConsumerAsync,
            cancellationToken: stoppingToken
        );
    }

    private async Task HandleMessageConsumerAsync(string message, ulong deliveryTag)
    {
        _logger.LogInformation($"Processando a mensagem: {message}");

        var authorImageCreateDto = JsonSerializer.Deserialize<AuthorImageCreateDto>(message);

        if (authorImageCreateDto is not AuthorImageCreateDto)
        {
            _logger.LogError($"Problemas para deserializar a mensagem {message}");

            throw new Exception($"Problemas para deserializar a mensagem {message}");
        }

        try
        {
            // Integrar com API para criação de imagem

            //_logger.LogInformation("############### Created Image #########################################");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
