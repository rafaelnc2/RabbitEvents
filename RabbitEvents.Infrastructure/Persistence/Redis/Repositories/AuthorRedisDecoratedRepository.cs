using RabbitEvents.Infrastructure.Shared;

namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

internal class AuthorRedisDecoratedRepository : IAuthorRedisRepository
{
    private readonly AuthorRedisRepository _autorRedisRepository;
    private readonly EventSender _eventSender;

    public AuthorRedisDecoratedRepository(AuthorRedisRepository autorRedisRepository, EventSender eventSender)
    {
        _autorRedisRepository = autorRedisRepository;
        _eventSender = eventSender;
    }

    public async Task<Author> CriarAsync(Author autor)
    {
        await _autorRedisRepository.CriarAsync(autor);

        await _eventSender.PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Author> AtualizarAsync(Author autor)
    {
        await _autorRedisRepository.AtualizarAsync(autor);

        await _eventSender.PublishEventsAsync(autor);

        return autor;
    }

    public Task<Author?> ObterPorIdAsync(string autorId) =>
        _autorRedisRepository.ObterPorIdAsync(autorId);

    public Task<IEnumerable<Author>> ObterTodosAsync(string? nameFilter) =>
        _autorRedisRepository.ObterTodosAsync(nameFilter);

}
