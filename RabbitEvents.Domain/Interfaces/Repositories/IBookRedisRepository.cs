namespace RabbitEvents.Domain.Interfaces.Repositories;

public interface IBookRedisRepository
{
    Task<Book> CriarAsync(Book book);

    Task<Book> AtualizarAsync(Book book);

    Task<Book?> ObterPorIdAsync(string bookId);

    Task<IEnumerable<Book>> ObterTodosAsync(string? filterTitle);
}
