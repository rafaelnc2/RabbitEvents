using Microsoft.AspNetCore.Http;

namespace RabbitEvents.Shared.Inputs.Books;

public record CreateBookInput(
    string AuthorId,
    string Titulo,
    string Prefacio,
    string Edicao,
    int AnoPublicacao,
    string Editora,
    string GeneroLiterario,
    double Preco,
    IFormFile? Imagem
);
