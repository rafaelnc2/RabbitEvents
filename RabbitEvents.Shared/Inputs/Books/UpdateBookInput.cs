using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace RabbitEvents.Shared.Inputs.Books;

public record UpdateBookInput(
    string Titulo,
    string Prefacio,
    string Edicao,
    int AnoPublicacao,
    string Editora,
    string GeneroLiterario,
    double Preco,
    IFormFile? Imagem
)
{
    [SwaggerSchema(ReadOnly = true)]
    public string? Id { get; set; }
};