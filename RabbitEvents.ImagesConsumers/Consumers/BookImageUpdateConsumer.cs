namespace RabbitEvents.ImagesConsumers.Consumers;

public sealed class BookImageUpdateConsumer : BackgroundService
{
    private readonly ILogger<AuthorImageUpdateConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly IServiceProvider _serviceProvider;

    private IBookRedisRepository? _bookRedisRepository;

    public BookImageUpdateConsumer(ILogger<AuthorImageUpdateConsumer> logger, IQueueService queueService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _queueService = queueService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(BookImageUpdateConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.BOOKS_IMAGE_UPDATE_QUEUE.Name,
            messageHandlerAsync: HandleMessageConsumerAsync,
            cancellationToken: stoppingToken
        );

        await Task.CompletedTask;
    }

    private async Task HandleMessageConsumerAsync(string message, ulong deliveryTag)
    {
        _logger.LogInformation($"Processando a mensagem: {message}");

        var messageBody = JsonSerializer.Deserialize<ImageMessageBodyDto>(message);

        if (messageBody is null)
        {
            _logger.LogError($"Problemas para deserializar a mensagem {message}");

            throw new Exception($"Problemas para deserializar a mensagem {message}");
        }

        if (string.IsNullOrWhiteSpace(messageBody.ImageId) || string.IsNullOrWhiteSpace(messageBody.FileExtension) || string.IsNullOrWhiteSpace(messageBody.ContentType))
        {
            _logger.LogError($"Erro ao obter dados da mensagem. Mensagem: {message}");

            throw new Exception($"Erro ao obter dados da mensagem. Mensagem: {message}");
        }

        string entityId = ProcessFileName.GetTextAfterSeparator(messageBody.ImageId, CacheKeysConstants.KEY_SEPARATOR);

        if (string.IsNullOrWhiteSpace(entityId) || GuidValidator.Validate(entityId) is false)
        {
            _logger.LogError($"ImageId inválido. Mensagem: {message}");

            throw new Exception($"ImageId inválido. Mensagem: {message}");
        }

        GetBookRepository();

        Book? book = await _bookRedisRepository!.ObterPorIdAsync(entityId);

        if (book is null)
        {
            _logger.LogError($"Autor com Id {entityId} não foi encontrado. Mensagem: {message}");

            throw new Exception($"Autor com Id {entityId} não foi encontrado. Mensagem: {message}");
        }

        string newFileName = $"{entityId}.{messageBody.FileExtension}";

        book.UpdateImageName(newFileName);

        await _bookRedisRepository.AtualizarAsync(book);
    }

    private void GetBookRepository()
    {
        using var scope = _serviceProvider.CreateScope();

        _bookRedisRepository = scope.ServiceProvider.GetRequiredService<IBookRedisRepository>();
    }
}
