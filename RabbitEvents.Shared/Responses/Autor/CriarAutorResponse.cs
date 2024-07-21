namespace RabbitEvents.Shared.Responses.Autor;

public sealed record CriarAutorResponse(Guid Id, string Nome, string Sobre, string Biografia, string? Imaegm, DateTime DataCriacao);
