using RabbitEvents.Domain.DomainEvents.AuthorEvents;
using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = ["Author"])]
public sealed class Author : Entity
{
    [JsonConstructor]
    private Author(Guid id, string nome, string sobre, string biografia, string genero, string? imagem, DateTime dataCriacao, DateTime? dataAtualizacao)
    {
        Id = id;

        Nome = nome;
        Sobre = sobre;
        Biografia = biografia;
        Genero = genero;
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

    [Indexed]
    public string Genero { get; private set; }

    public string? Imagem { get; private set; }

    [JsonIgnore]
    public List<Livro>? Livros { get; private set; }


    public static Author Create(string nome, string sobre, string biografia, string genero)
    {
        var newId = Guid.NewGuid();

        var autor = new Author(
            id: newId,

            nome: nome.Trim(),
            sobre: sobre.Trim(),
            biografia: biografia.Trim(),
            genero: genero,
            imagem: null,

            dataCriacao: DateTime.Now,
            dataAtualizacao: null
        );

        Raise(new AuthorCreatedEvent(autor.Id));

        return autor;
    }

    public void Update(string nome, string sobre, string biografia, string genero)
    {
        Nome = nome.Trim();
        Sobre = sobre.Trim();
        Biografia = biografia.Trim();
        Genero = genero.Trim();

        DataAtualizacao = DateTime.Now;

        Raise(new AuthorUpdatedEvent(Id));
    }


    public void UpdateImageName(string imageName)
    {
        Imagem = imageName;

        DataAtualizacao = DateTime.Now;
    }
}
