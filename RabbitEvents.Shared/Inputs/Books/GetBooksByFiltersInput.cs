namespace RabbitEvents.Shared.Inputs.Books;

public record GetBooksByFiltersInput(string? Titulo, string? Editora, string? GeneroLiterario, string? IdAutor, string? NomeAutor);
