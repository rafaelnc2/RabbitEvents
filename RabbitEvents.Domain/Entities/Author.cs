using RabbitEvents.Domain.DomainEvents.AuthorEvents;
using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = ["Author"])]
public sealed class Author : Entity
{
    [JsonConstructor]
    private Author(Guid id, string nome, string sobre, string biografia, string? imagem, DateTime dataCriacao, DateTime? dataAtualizacao)
    {
        Id = id;

        Nome = nome;
        Sobre = sobre;
        Biografia = biografia;
        Imagem = imagem;

        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
    }

    [Searchable]
    public string Nome { get; private set; }

    [Searchable]
    public string Sobre { get; private set; }

    [Searchable]
    public string Biografia { get; private set; }

    public string? Imagem { get; private set; }

    [JsonIgnore]
    public List<Book>? Livros { get; private set; }


    public static Author Create(string nome, string sobre, string biografia)
    {
        var newId = Guid.NewGuid();

        var autor = new Author(
            id: newId,

            nome: nome.Trim(),
            sobre: sobre.Trim(),
            biografia: biografia.Trim(),
            imagem: null,

            dataCriacao: DateTime.Now,
            dataAtualizacao: null
        );

        Raise(new AuthorCreatedEvent(autor.Id));

        return autor;
    }

    public void Update(string nome, string sobre, string biografia)
    {
        Nome = nome.Trim();
        Sobre = sobre.Trim();
        Biografia = biografia.Trim();

        DataAtualizacao = DateTime.Now;

        Raise(new AuthorUpdatedEvent(Id));
    }


    public void UpdateImageName(string imageName)
    {
        Imagem = imageName;

        DataAtualizacao = DateTime.Now;
    }
}
