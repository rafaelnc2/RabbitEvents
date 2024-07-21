using RabbitEvents.Domain.Entities;
using RabbitEvents.Shared.Inputs.Livros;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface ILivroDomainService
{
    ValueTask<Livro> CriarAsync(CriarLivroInput criarInput);
    ValueTask<Livro> AtualizarAsync(AtualizarLivroInput atualizarInput);

    ValueTask<IEnumerable<Livro>> ObterTodosAsync();
    ValueTask<Livro> ObterPorIdAsync(string id);
}
