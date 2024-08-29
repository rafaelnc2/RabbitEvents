using Microsoft.AspNetCore.Http;

namespace RabbitEvents.Shared.Inputs.Authors;

public record CreateAuthorInput(string Nome, string Sobre, string Biografia, IFormFile? Imagem);
