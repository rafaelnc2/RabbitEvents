using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Application.Mappings;

public static class BookMap
{
    public static CreateBookResponse ToCreateBookRespponse(Book book) =>
        new CreateBookResponse(
            book.Id,
            book.Titulo,
            book.Prefacio,
            book.Author.Nome,
            book.Edicao,
            book.AnoPublicacao,
            book.Editora,
            book.GeneroLiterario,
            book.Preco
        );

}
