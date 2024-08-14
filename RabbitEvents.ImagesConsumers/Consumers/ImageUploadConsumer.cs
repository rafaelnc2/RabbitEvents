using RabbitEvents.Application.Interfaces;
using RabbitEvents.ImagesConsumers.Helpers;
using RabbitEvents.Shared.Constants;
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

        string fileNameMessage = $"{messageBody.ImageId}.{messageBody.FileExtension}";

        string fileName = ProcessFileName.GetTextAfterSeparator(fileNameMessage, CacheKeysConstants.KEY_SEPARATOR);

        using Stream stream = new MemoryStream(cachedAutorImage);

        using Stream resizedImageStream = await ResizeImageAsync(stream, fileName);

        var uploadedFile = await _blobService.UploadFileAsync(BlobStorageConstants.AuthoImageContainerName, resizedImageStream, fileName, messageBody.ContentType, CancellationToken.None);

        SendImageUpdateToQueue(message);

        await _cacheService.DeleteValueAsync(messageBody.ImageId);
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

    private void SendImageUpdateToQueue(string messageBody)
    {
        _queueService.SendMessage(new QueueMessage(
            Queue: QueueDefinitions.AUTHORS_IMAGE_UPDATE_QUEUE,
            Exchange: QueueDefinitions.AUTHORS_EXCHANGE,
            RoutingKey: QueueDefinitions.AUTHORS_IMAGE_UPDATE_QUEUE.RoutingKey,
            MessageBody: messageBody
        ));
    }
}
