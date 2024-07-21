namespace RabbitEvents.Domain.Interfaces.Repositories;

public interface IAutorRedisRepository
{
    Task<Autor> CriarAsync(Autor autor);

    Task<Autor> CriarAsync(Autor autor, byte[]? imageInBytes);

    Task<Autor> AtualizarAsync(Autor autor);

    Task<Autor?> ObterPorIdAsync(string autorId);

    Task<IEnumerable<Autor>> ObterTodosAsync();
}
