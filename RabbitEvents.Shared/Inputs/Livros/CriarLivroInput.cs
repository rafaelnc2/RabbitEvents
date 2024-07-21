namespace RabbitEvents.Shared.Inputs.Livros;

public record CriarLivroInput(string Titulo, string Descricao, Guid AutorId, string Edicao, int AnoPublicacao, string Editora, double Preco);
