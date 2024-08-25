namespace RabbitEvents.Shared.Responses.Autor;

public record AuthorResponse(Guid Id, string Nome, string Sobre, string Biografia, string Genero,
    string? Imagem, DateTime DataCriacao, DateTime? DataAtulizacao);
