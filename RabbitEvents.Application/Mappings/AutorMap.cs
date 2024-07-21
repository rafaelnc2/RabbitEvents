using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Mappings;

public static class AutorMap
{
    public static AutorResponse ToAutorResponse(Autor autor) =>
        new AutorResponse(
            autor.Id,
            autor.Nome,
            autor.Sobre,
            autor.Biografia,
            autor.Imagem,
            autor.DataCriacao,
            autor.DataAtualizacao
        );

    public static CriarAutorResponse ToCriarAutorResponse(Autor autor) =>
        new CriarAutorResponse(
            autor.Id,
            autor.Nome,
            autor.Sobre,
            autor.Biografia,
            autor.Imagem,
            autor.DataCriacao
        );
}
