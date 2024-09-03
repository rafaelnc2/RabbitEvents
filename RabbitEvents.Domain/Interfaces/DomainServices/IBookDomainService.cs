using RabbitEvents.Shared.Inputs.Books;
using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface IBookDomainService
{
    Task<ApiResponse<CreateBookResponse>> CriarAsync(CreateBookInput criarInput);
    Task<Book> AtualizarAsync(UpdateBookInput atualizarInput);

    Task<IEnumerable<Book>> ObterTodosAsync();
    Task<Book> ObterPorIdAsync(string id);
}
