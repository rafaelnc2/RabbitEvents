namespace RabbitEvents.Shared.Responses.Books;

public record CreateBookResponse(
    Guid Id,
    string Titulo,
    string Prefacio,
    string NomeAutor,
    string Edicao,
    int AnoPublicacao,
    string Editora,
    string GeneroLiterario,
    double Preco
);