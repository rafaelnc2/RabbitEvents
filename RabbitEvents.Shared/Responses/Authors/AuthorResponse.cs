namespace RabbitEvents.Shared.Responses.Authors;

public record AuthorResponse(Guid Id, string Nome, string Sobre, string Biografia,
    string? Imagem, DateTime DataCriacao, DateTime? DataAtulizacao);
