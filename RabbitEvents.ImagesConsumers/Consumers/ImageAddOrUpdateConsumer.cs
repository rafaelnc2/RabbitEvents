using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

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

        var cachedAutorImage = await _cacheService.GetBytesValueAsync(messageBody.ImageId);

        if (cachedAutorImage is null)
        {
            _logger.LogWarning($"A imagem {messageBody.ImageId} não existe no cache");
            return;
        }

        string fileName = ProcessFileName($"{messageBody.ImageId}.{messageBody.FileExtension}");

        using Stream stream = new MemoryStream(cachedAutorImage);

        using Stream resizedImageStream = await ResizeImageAsync(stream, fileName);

        var uploadedFile = await _blobService.UploadFileAsync(BlobContainerName, resizedImageStream, fileName, messageBody.ContentType, CancellationToken.None);

        await _cacheService.DeleteValueAsync(messageBody.ImageId);
    }

    private string ProcessFileName(string currentFileName)
    {
        var currentFileNameSpan = currentFileName.AsSpan();

        var charIndex = currentFileNameSpan.IndexOf(':');

        var newFileName = currentFileNameSpan[(charIndex + 1)..];

        return newFileName.ToString();
    }

    private async Task<Stream> ResizeImageAsync(Stream imageStream, string fileName)
    {
        var newImageStream = new MemoryStream();

        using (var image = await Image.LoadAsync(imageStream))
        {
            image.Mutate(img => img.Resize(200, 200));
            await image.SaveAsync(newImageStream, image.DetectEncoder(fileName));
        }

        newImageStream.Position = 0;

        return newImageStream;
    }
}
