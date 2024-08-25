namespace RabbitEvents.Shared.Responses.Autor;

public sealed record CreateAuthorResponse(Guid Id, string Nome, string Sobre, string Biografia, string Genero, string? Imaegm, DateTime DataCriacao);
