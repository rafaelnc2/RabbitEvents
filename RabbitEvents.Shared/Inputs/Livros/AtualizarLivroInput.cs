namespace RabbitEvents.Shared.Inputs.Livros;

public record AtualizarLivroInput(Guid Id, string Titulo, string Descricao, Guid AutorId, string Edicao, int AnoPublicacao, string Editora, double Preco);