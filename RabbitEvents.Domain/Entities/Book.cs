using RabbitEvents.Domain.DomainEvents.BookEvents;
using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = ["Book"])]
public sealed class Book : Entity
{
    [JsonConstructor]
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

    [Searchable]
    public string Titulo { get; private set; }

    [Searchable]
    public string Prefacio { get; private set; }

    [Searchable]
    public string Edicao { get; private set; }

    [Searchable]
    public int AnoPublicacao { get; set; }

    [Searchable]
    public string Editora { get; private set; }

    [Indexed]
    public string GeneroLiterario { get; private set; }

    [Searchable]
    public double Preco { get; private set; }

    public string? Imagem { get; private set; }


    [JsonIgnore]
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

        Raise(new BookCreatedEvent(livro.Id));

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

    public void UpdateImageName(string imageName)
    {
        Imagem = imageName;

        DataAtualizacao = DateTime.Now;
    }
}
