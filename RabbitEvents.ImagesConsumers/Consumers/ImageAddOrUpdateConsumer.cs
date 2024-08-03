using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Constants;
using RabbitEvents.Shared.Dtos;
using System.Text.Json;

namespace RabbitEvents.ImagesConsumers.Consumers;

public sealed class ImageAddOrUpdateConsumer : BackgroundService
{
    private readonly ILogger<ImageAddOrUpdateConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICacheService _cacheService;
    private readonly IBlobService _blobService;

    private const string BlobContainerName = "author-images";

    public ImageAddOrUpdateConsumer(ILogger<ImageAddOrUpdateConsumer> logger,
        IQueueService queueService,
        IServiceProvider serviceProvider,
        ICacheService cacheService,
        IBlobService blobService)
    {
        _logger = logger;
        _queueService = queueService;
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
        _blobService = blobService;
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

        var messageBody = JsonSerializer.Deserialize<ImageMessageBodyDto>(message);

        if (messageBody is null)
        {
            _logger.LogError($"Problemas para deserializar a mensagem {message}");
            return;
        }

        if (string.IsNullOrWhiteSpace(messageBody.ImageId) || string.IsNullOrWhiteSpace(messageBody.FileExtension) || string.IsNullOrWhiteSpace(messageBody.ContentType))
        {
            _logger.LogError($"Erro ao obter dados da imagem. Mensagem: {message}");
            return;
        }

        var authorIdCacheKey = $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{messageBody.ImageId}";

        var cachedAutorImage = await _cacheService.GetBytesValueAsync(authorIdCacheKey);

        if (cachedAutorImage is null)
        {
            _logger.LogWarning($"A imagem {authorIdCacheKey} não existe no cache");
            return;
        }

        string fileName = $"{messageBody.ImageId}.{messageBody.FileExtension}";

        using Stream stream = new MemoryStream(cachedAutorImage);

        var uploadedFile = await _blobService.UploadFileAsync(BlobContainerName, stream, fileName, messageBody.ContentType, CancellationToken.None);

        await _cacheService.DeleteValueAsync(authorIdCacheKey);
    }
}
