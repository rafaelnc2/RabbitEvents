using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace RabbitEvents.Shared.Inputs.Autor;

public record AtualizarAutorInput(string Nome, string Sobre, string Biografia, IFormFile? Imagem)
{
    [SwaggerSchema(ReadOnly = true)]
    public string? Id { get; set; }
};
