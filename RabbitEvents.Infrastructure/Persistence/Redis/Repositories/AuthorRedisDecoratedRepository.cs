namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

internal class AuthorRedisDecoratedRepository : IAutorRedisRepository
{
    private readonly AuthorRedisRepository _autorRedisRepository;
    private readonly IBus _bus;

    public AuthorRedisDecoratedRepository(AuthorRedisRepository autorRedisRepository, IBus bus)
    {
        _autorRedisRepository = autorRedisRepository;
        _bus = bus;
    }

    public async Task<Author> CriarAsync(Author autor)
    {
        await _autorRedisRepository.CriarAsync(autor);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Author> CriarAsync(Author autor, byte[]? imageInBytes)
    {
        await _autorRedisRepository.CriarAsync(autor, imageInBytes);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Author> AtualizarAsync(Author autor)
    {
        await _autorRedisRepository.AtualizarAsync(autor);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Author> AtualizarAsync(Author autor, byte[]? imageInBytes)
    {
        await _autorRedisRepository.AtualizarAsync(autor, imageInBytes);

        await PublishEventsAsync(autor);

        return autor;
    }


    public Task<Author?> ObterPorIdAsync(string autorId) =>
        _autorRedisRepository.ObterPorIdAsync(autorId);

    public Task<IEnumerable<Author>> ObterTodosAsync() =>
        _autorRedisRepository.ObterTodosAsync();


    private async ValueTask PublishEventsAsync(Entity entity)
    {
        if (entity.DomainEvents.Any() is false)
            return;

        foreach (IDomainEvent @event in entity.DomainEvents)
        {
            await _bus.Publish((object)@event);
        }

        entity.ClearEvents();
    }
}
