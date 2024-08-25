using OpenAI.Images;
using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Constants;

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
            var imageCreatorClient = new ImageClient("dall-e-3", _apiKey);

            var imageRequest = new ImageGenerationOptions()
            {
                Quality = GeneratedImageQuality.High,
                Size = GeneratedImageSize.W1024xH1792,
                Style = GeneratedImageStyle.Natural,
                ResponseFormat = GeneratedImageFormat.Uri
            };

            string templateToCreateImage = $"I want an image of {authorImageCreateDto.AuthorName} as a cartoon, very smiling, reading a book and drinking a cup of coffee";

            var response = await imageCreatorClient.GenerateImageAsync(templateToCreateImage, imageRequest);

            _logger.LogInformation("############### Created Image #########################################");
            _logger.LogInformation(response.Value.ImageUri.ToString());
            _logger.LogInformation("############### Created Image #########################################");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
