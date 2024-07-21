using Microsoft.AspNetCore.Http;

namespace RabbitEvents.Shared.Inputs.Autor;

public record CriarAutorInput(string Nome, string Sobre, string Biografia, IFormFile? Imagem);
