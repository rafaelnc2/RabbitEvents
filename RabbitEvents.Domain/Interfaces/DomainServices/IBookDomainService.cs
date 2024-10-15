using RabbitEvents.Shared.Inputs.Authors;
using RabbitEvents.Shared.Inputs.Books;
using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface IBookDomainService
{
    Task<ApiResponse<BookResponse>> CriarAsync(CreateBookInput criarInput);
    Task<ApiResponse<BookResponse>> AtualizarAsync(UpdateBookInput atualizarInput);

    Task<ApiResponse<BookResponse>> ObterPorIdAsync(GetBookByIdInput obterLivroPorIdInput);
    Task<ApiResponse<IEnumerable<BookResponse>>> ObterTodosAsync(GetBooksByFiltersInput? filtersInput);

    Task<ApiResponse<IEnumerable<string>>> ObterGenerosLiterariosAsync();
}
