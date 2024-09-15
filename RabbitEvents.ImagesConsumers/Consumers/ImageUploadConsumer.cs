using RabbitEvents.Shared.Models.Messaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace RabbitEvents.ImagesConsumers.Consumers;

public sealed class ImageUploadConsumer : BackgroundService
{
    private readonly ILogger<ImageUploadConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly ICacheService _cacheService;
    private readonly IBlobService _blobService;

    public ImageUploadConsumer(ILogger<ImageUploadConsumer> logger,
        IQueueService queueService,
        ICacheService cacheService,
        IBlobService blobService)
    {
        _logger = logger;
        _queueService = queueService;
        _cacheService = cacheService;
        _blobService = blobService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(ImageUploadConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.IMAGES_UPLOAD_QUEUE.Name,
            messageHandlerAsync: HandleMessageConsumerAsync,
            cancellationToken: stoppingToken
        );
    }

    private async Task HandleMessageConsumerAsync(string message, ulong deliveryTag)
    {
        _logger.LogInformation($"Processando a mensagem: {message}");

        var queuesMessageDto = JsonSerializer.Deserialize<ImageMessageBodyDto>(message);

        if (queuesMessageDto is null)
        {
            _logger.LogError($"Problemas para deserializar a mensagem {message}");

            throw new Exception($"Problemas para deserializar a mensagem {message}");
        }

        if (string.IsNullOrWhiteSpace(queuesMessageDto.ImageId) || string.IsNullOrWhiteSpace(queuesMessageDto.FileExtension) || string.IsNullOrWhiteSpace(queuesMessageDto.ContentType))
        {
            _logger.LogError($"Erro ao obter dados da imagem. Mensagem: {message}");

            throw new Exception($"Erro ao obter dados da imagem. Mensagem: {message}");
        }

        var cachedImage = await _cacheService.GetBytesValueAsync(queuesMessageDto.ImageId);

        if (cachedImage is null)
        {
            _logger.LogWarning($"A imagem {queuesMessageDto.ImageId} não existe no cache");

            throw new Exception($"A imagem {queuesMessageDto.ImageId} não existe no cache");
        }

        string fileNameMessage = $"{queuesMessageDto.ImageId}.{queuesMessageDto.FileExtension}";

        string fileName = ProcessFileName.GetTextAfterSeparator(fileNameMessage, CacheKeysConstants.KEY_SEPARATOR);

        using Stream stream = new MemoryStream(cachedImage);

        using Stream resizedImageStream = await ResizeImageAsync(stream, fileName);

        var uploadedFile = await _blobService.UploadFileAsync(
            queuesMessageDto.BlobContainerName,
            resizedImageStream,
            fileName,
            queuesMessageDto.ContentType,
            CancellationToken.None
        );

        SendImageUpdateToQueue(queuesMessageDto, message);

        await _cacheService.DeleteValueAsync(queuesMessageDto.ImageId);
    }

    private async Task<Stream> ResizeImageAsync(Stream imageStream, string fileName)
    {
        var newImageStream = new MemoryStream();

        using (var image = await Image.LoadAsync(imageStream))
        {
            image.Mutate(img => img.Resize(ImageDefinitions.DEFAULT_WIDTH, ImageDefinitions.DEFAULT_HEIGHT));
            await image.SaveAsync(newImageStream, image.DetectEncoder(fileName));
        }

        newImageStream.Position = 0;

        return newImageStream;
    }

    private void SendImageUpdateToQueue(ImageMessageBodyDto queuesMessageDto, string messageToSend)
    {
        _queueService.SendMessage(new QueueMessage(
            Queue: queuesMessageDto.DestinationQueue,
            Exchange: queuesMessageDto.DestinationExchange,
            RoutingKey: queuesMessageDto.DestinationQueue.RoutingKey,
            MessageBody: messageToSend
        ));
    }
}
