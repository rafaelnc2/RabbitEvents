using RabbitEvents.Application.Interfaces;
using RabbitEvents.Shared.Configurations;
using RabbitEvents.Shared.Constants;
using StackExchange.Redis;

namespace RabbitEvents.BooksConsumers.Consumers;

public sealed class LiteraryGenreListUpdateConsumer : BackgroundService
{
    private readonly ILogger<LiteraryGenreListUpdateConsumer> _logger;
    private readonly IQueueService _queueService;
    private readonly IDatabase _database;

    public LiteraryGenreListUpdateConsumer(IConnectionMultiplexer redisMultiplexerConnect,
        ILogger<LiteraryGenreListUpdateConsumer> logger, IQueueService queueService)
    {
        _database = redisMultiplexerConnect.GetDatabase();
        _logger = logger;
        _queueService = queueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"####################### Iniciando {nameof(LiteraryGenreListUpdateConsumer)} ##################################");

        var message = _queueService.ConsumeQueue(
            queueName: QueueDefinitions.LITERARY_GENRE_QUEUE.Name,
            messageHandlerAsync: HandleMessageConsumerAsync,
            cancellationToken: stoppingToken
        );

        await Task.CompletedTask;
    }

    private async Task HandleMessageConsumerAsync(string message, ulong deliveryTag)
    {
        _logger.LogInformation($"Processando a mensagem: {message}");

        if (message is null)
        {
            _logger.LogError($"Mensagem inválida: {message}");

            throw new Exception($"Mensagem inválida: {message}");
        }

        ReadOnlyMemory<char> lieteraryGenres = message.AsMemory();

        int index = 0;
        int qtdPipesInMessage = CountPipes(lieteraryGenres);
        var lieteraryGenreList = new List<string>();

        while (index < lieteraryGenres.Length)
        {
            bool hasError = false;
            int pipeIndex = lieteraryGenres.Span.Slice(index).IndexOf(CacheKeysConstants.LITERARY_GENRE_SEPARATOR);

            string valueToAdd = GetSlicedData(lieteraryGenres, index, pipeIndex);

            if (string.IsNullOrWhiteSpace(valueToAdd))
            {
                _logger.LogWarning($"Valor inválido");

                hasError = true;
            }

            if (valueToAdd.Length > CacheKeysConstants.LITERARY_GENRE_MAX_LENGTH)
            {
                _logger.LogWarning($"O valor {valueToAdd} não foi inserido pois é maior do que o máximo permitido: {CacheKeysConstants.LITERARY_GENRE_MAX_LENGTH}");

                hasError = true;
            }

            if (lieteraryGenreList.Contains(valueToAdd))
            {
                _logger.LogWarning($"O valor {valueToAdd} já existe na lista a ser inserida");

                hasError = true;
            }

            if (hasError is false)
                await InsertValueAsync(valueToAdd);

            index += pipeIndex + 1;

            if (pipeIndex == -1)
                break;
        }
    }

    private string GetSlicedData(ReadOnlyMemory<char> messageMemory, int currentIndex, int pipeIndex)
    {
        if (pipeIndex == -1)
            return new String(messageMemory.Slice(currentIndex).Trim().Span);

        return new String(messageMemory.Slice(currentIndex, pipeIndex).Trim().Span);
    }

    private int CountPipes(ReadOnlyMemory<char> messageMemory)
    {
        int count = 0;
        var span = messageMemory.Span;

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '|')
                count++;
        }

        return count;
    }

    private Task InsertValueAsync(string valueToAdd)
    {
        _logger.LogInformation($"Inserindo: {valueToAdd}");

        return _database.SetAddAsync(CacheKeysConstants.LITERARY_GENRE_LIST_NAME, new RedisValue[] { valueToAdd });
    }
}
