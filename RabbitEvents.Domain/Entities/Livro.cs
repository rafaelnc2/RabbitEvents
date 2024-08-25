namespace RabbitEvents.Domain.Entities;

public sealed class Livro : Entity
{
    private Livro(Guid id, string titulo, string descricao, Author autor, string edicao, int anoPublicacao, string editora, double preco,
        DateTime dataCriacao, DateTime? dataAtualizacao)
    {
        Id = id;

        Titulo = titulo;
        Descricao = descricao;
        Autor = autor;
        Edicao = edicao;
        AnoPublicacao = anoPublicacao;
        Editora = editora;
        Preco = preco;

        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
    }

    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public Author Autor { get; private set; }
    public string Edicao { get; private set; }
    public int AnoPublicacao { get; set; }
    public string Editora { get; private set; }
    public double Preco { get; private set; }


    public static Livro Create(string titulo, string descricao, Author autor, string edicao, int anoPublicacao, string editora, double preco)
    {
        var livro = new Livro(
            id: Guid.NewGuid(),
            titulo: titulo,
            descricao: descricao,
            autor: autor,
            edicao: edicao,
            anoPublicacao: anoPublicacao,
            editora: editora,
            preco: preco,
            dataCriacao: DateTime.Now,
            dataAtualizacao: null
        );

        return livro;
    }

    public void Update(string titulo, string descricao, Author autor, string edicao, int anoPublicacao, string editora, double preco)
    {
        Titulo = titulo;
        Descricao = descricao;
        Autor = autor;
        Edicao = edicao;
        AnoPublicacao = anoPublicacao;
        Editora = editora;
        Preco = preco;
        DataAtualizacao = DateTime.Now;
    }
}
