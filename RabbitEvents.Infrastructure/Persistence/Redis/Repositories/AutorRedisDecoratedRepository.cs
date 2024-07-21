namespace RabbitEvents.Infrastructure.Persistence.Redis.Repositories;

internal class AutorRedisDecoratedRepository : IAutorRedisRepository
{
    private readonly AutorRedisRepository _autorRedisRepository;
    private readonly IBus _bus;

    public AutorRedisDecoratedRepository(AutorRedisRepository autorRedisRepository, IBus bus)
    {
        _autorRedisRepository = autorRedisRepository;
        _bus = bus;
    }

    public async Task<Autor> CriarAsync(Autor autor)
    {
        await _autorRedisRepository.CriarAsync(autor);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Autor> CriarAsync(Autor autor, byte[]? imageInBytes)
    {
        await _autorRedisRepository.CriarAsync(autor, imageInBytes);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Autor> AtualizarAsync(Autor autor)
    {
        await _autorRedisRepository.AtualizarAsync(autor);

        await PublishEventsAsync(autor);

        return autor;
    }

    public async Task<Autor> AtualizarAsync(Autor autor, byte[]? imageInBytes)
    {
        await _autorRedisRepository.AtualizarAsync(autor, imageInBytes);

        await PublishEventsAsync(autor);

        return autor;
    }


    public Task<Autor?> ObterPorIdAsync(string autorId) =>
        _autorRedisRepository.ObterPorIdAsync(autorId);

    public Task<IEnumerable<Autor>> ObterTodosAsync() =>
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
