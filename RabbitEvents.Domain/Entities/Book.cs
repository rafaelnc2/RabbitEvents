using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitEvents.Domain.Entities;

public sealed class Book : Entity
{
    private Book(Guid id, Guid authorId, string titulo, string prefacio, string edicao, int anoPublicacao, string editora, string generoLiterario, double preco,
        DateTime dataCriacao, DateTime? dataAtualizacao)
    {
        Id = id;
        AuthorId = authorId;
        Titulo = titulo;
        Prefacio = prefacio;
        Edicao = edicao;
        AnoPublicacao = anoPublicacao;
        Editora = editora;
        GeneroLiterario = generoLiterario;
        Preco = preco;

        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
    }

    public Guid AuthorId { get; private set; }
    public string Titulo { get; private set; }
    public string Prefacio { get; private set; }
    public string Edicao { get; private set; }
    public int AnoPublicacao { get; set; }
    public string Editora { get; private set; }
    public string GeneroLiterario { get; private set; }
    public double Preco { get; private set; }

    [NotMapped]
    public Author Author { get; private set; }

    public static Book Create(Author author, string titulo, string prefacio, string edicao, int anoPublicacao, string editora, string generoLiterario, double preco)
    {
        var livro = new Book(
            id: Guid.NewGuid(),
            authorId: author.Id,
            titulo: titulo,
            prefacio: prefacio,
            edicao: edicao,
            anoPublicacao: anoPublicacao,
            editora: editora,
            generoLiterario: generoLiterario,
            preco: preco,
            dataCriacao: DateTime.Now,
            dataAtualizacao: null
        );

        livro.Author = author;

        return livro;
    }

    public void Update(Guid authorId, string titulo, string prefacio, string edicao, int anoPublicacao, string editora, string generoLiterario, double preco)
    {
        AuthorId = authorId;
        Titulo = titulo;
        Prefacio = prefacio;
        Edicao = edicao;
        AnoPublicacao = anoPublicacao;
        Editora = editora;
        GeneroLiterario = generoLiterario;
        Preco = preco;
        DataAtualizacao = DateTime.Now;
    }
}
