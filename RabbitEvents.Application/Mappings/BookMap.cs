using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Application.Mappings;

public static class BookMap
{
    public static BookResponse ToCreateBookRespponse(Book book) =>
        new BookResponse(
            book.Id,
            book.Titulo,
            book.Prefacio,
            book.AuthorInfo.Nome,
            book.Edicao,
            book.AnoPublicacao,
            book.Editora,
            book.GeneroLiterario,
            book.Preco
        );

}
