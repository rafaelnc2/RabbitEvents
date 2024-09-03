namespace RabbitEvents.Shared.Responses.Authors;

public sealed record CreateAuthorResponse(Guid Id, string Nome, string Sobre, string Biografia, string? Imagem, DateTime DataCriacao);
