using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Mappings;

public static class AuthorMap
{
    public static AuthorResponse ToAutorResponse(Author autor) =>
        new AuthorResponse(
            autor.Id,
            autor.Nome,
            autor.Sobre,
            autor.Biografia,
            string.IsNullOrWhiteSpace(autor.Imagem) ? "default.jpeg" : autor.Imagem,
            autor.DataCriacao,
            autor.DataAtualizacao
        );

    public static CreateAuthorResponse ToCriarAutorResponse(Author autor) =>
        new CreateAuthorResponse(
            autor.Id,
            autor.Nome,
            autor.Sobre,
            autor.Biografia,
            string.IsNullOrWhiteSpace(autor.Imagem) ? "default.jpeg" : autor.Imagem,
            autor.DataCriacao
        );
}
