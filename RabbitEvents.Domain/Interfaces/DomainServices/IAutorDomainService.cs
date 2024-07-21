using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface IAutorDomainService
{
    Task<ApiResponse<CriarAutorResponse>> CriarAsync(CriarAutorInput criarInput);

    Task<ApiResponse<AutorResponse>> AtualizarAsync(AtualizarAutorInput atualizarInput);

    Task<ApiResponse<AutorResponse>> ObterPorIdAsync(ObterAutorPorIdInput obterAutorPorIdInput);

    Task<ApiResponse<IEnumerable<AutorResponse>>> ObterTodosAsync();
}
