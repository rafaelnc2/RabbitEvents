using RabbitEvents.Application.Interfaces;
using RabbitEvents.Application.Validators;
using RabbitEvents.Domain.Entities;
using RabbitEvents.Domain.Interfaces.Repositories;
using RabbitEvents.ImagesConsumers.Helpers;
using RabbitEvents.Shared.Constants;

namespace RabbitEvents.ImagesConsumers.Consumers;

/*
    Consumidor da fila que atualiza o nome da imagem do autor após upload do arquivo
 */

public sealed class AuthorImageUpdateConsumer : BackgroundService
{
    private readonly ILogger<AuthorImageUpdateConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly IServiceProvider _serviceProvider;

    private IAutorRedisRepository? _autorRedisRepository;

    public AuthorImageUpdateConsumer(ILogger<AuthorImageUpdateConsumer> logger, IQueueService queueService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _queueService = queueService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(AuthorImageUpdateConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.AUTHORS_IMAGE_UPDATE_QUEUE.Name,
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

        GetAutorRepository();

        Author? author = await _autorRedisRepository!.ObterPorIdAsync(entityId);

        if (author is null)
        {
            _logger.LogError($"Autor com Id {entityId} não foi encontrado. Mensagem: {message}");

            throw new Exception($"Autor com Id {entityId} não foi encontrado. Mensagem: {message}");
        }

        string newFileName = $"{entityId}.{messageBody.FileExtension}";

        author.UpdateImageName(newFileName);

        await _autorRedisRepository.AtualizarAsync(author);
    }

    private void GetAutorRepository()
    {
        using var scope = _serviceProvider.CreateScope();

        _autorRedisRepository = scope.ServiceProvider.GetRequiredService<IAutorRedisRepository>();
    }

}
