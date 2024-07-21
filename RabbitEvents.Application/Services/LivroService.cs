using RabbitEvents.Shared.Inputs.Livros;

namespace RabbitEvents.Application.Services;

public sealed class LivroService : ILivroDomainService
{
    public LivroService()
    {

    }

    public ValueTask<Livro> AtualizarAsync(AtualizarLivroInput atualizarInput)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Livro> CriarAsync(CriarLivroInput criarInput)
    {
        throw new NotImplementedException();
    }



    public ValueTask<Livro> ObterPorIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<Livro>> ObterTodosAsync()
    {
        throw new NotImplementedException();
    }
}
