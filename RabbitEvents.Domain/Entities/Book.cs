using RabbitEvents.Domain.DomainEvents.BookEvents;
using RabbitEvents.Domain.ValueObjects;
using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = ["Books"])]
public sealed class Book : Entity
{
    [JsonConstructor]
    private Book(Guid id, string titulo, string prefacio, string edicao, int anoPublicacao, string editora, string generoLiterario, double preco,
        AuthorInfoVo authorInfo, DateTime dataCriacao, DateTime? dataAtualizacao)
    {
        Id = id;
        Titulo = titulo;
        Prefacio = prefacio;
        Edicao = edicao;
        AnoPublicacao = anoPublicacao;
        Editora = editora;
        GeneroLiterario = generoLiterario;
        Preco = preco;

        AuthorInfo = authorInfo;

        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
    }

    [Indexed(Sortable = true)]
    public string Titulo { get; private set; }

    public string Prefacio { get; private set; }

    public string Edicao { get; private set; }

    public int AnoPublicacao { get; set; }

    [Searchable]
    public string Editora { get; private set; }

    [Indexed]
    public string GeneroLiterario { get; private set; }

    [Indexed(Sortable = true)]
    public double Preco { get; private set; }

    public string? Imagem { get; private set; }

    //[Indexed(JsonPath = "$.Id")]
    [Indexed(CascadeDepth = 1)]
    public AuthorInfoVo AuthorInfo { get; private set; }


    public static Book Create(string AuthorId, string AuthorName, string titulo, string prefacio, string edicao, int anoPublicacao, string editora,
        string generoLiterario, double preco)
    {
        var livro = new Book(
            id: Guid.NewGuid(),
            authorInfo: new AuthorInfoVo(AuthorId, AuthorName),
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

        Raise(new BookCreatedEvent(livro.Id));

        return livro;
    }

    public void Update(string titulo, string prefacio, string edicao, int anoPublicacao, string editora, string generoLiterario, double preco)
    {
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
