namespace RabbitEvents.Shared.Responses.Autor;

public record AutorResponse(Guid Id, string Nome, string Sobre, string Biografia, string? Imagem, DateTime DataCriacao, DateTime? DataAtulizacao);
