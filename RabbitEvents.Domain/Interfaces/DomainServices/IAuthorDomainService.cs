using RabbitEvents.Shared.Inputs.Authors;
using RabbitEvents.Shared.Responses.Authors;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface IAuthorDomainService
{
    Task<ApiResponse<CreateAuthorResponse>> CriarAsync(CreateAuthorInput criarInput);

    Task<ApiResponse<AuthorResponse>> AtualizarAsync(UpdateAuthorInput atualizarInput);

    Task<ApiResponse<AuthorResponse>> ObterPorIdAsync(GetAuthorByIdInput obterAutorPorIdInput);

    Task<ApiResponse<IEnumerable<AuthorResponse>>> ObterTodosAsync(string? nameFilter);
}
