namespace RabbitEvents.Shared.Inputs.Books;

public record UpdateBookInput(Guid Id, string Titulo, string Descricao, Guid AutorId, string Edicao, int AnoPublicacao, string Editora, double Preco);