namespace RabbitEvents.Domain.Interfaces.Repositories;

public interface IAutorRedisRepository
{
    Task<Author> CriarAsync(Author autor);

    Task<Author> CriarAsync(Author autor, byte[]? imageInBytes);

    Task<Author> AtualizarAsync(Author autor);

    Task<Author> AtualizarAsync(Author autor, byte[]? imageInBytes);

    Task<Author?> ObterPorIdAsync(string autorId);

    Task<IEnumerable<Author>> ObterTodosAsync();
}
