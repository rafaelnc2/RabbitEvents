using RabbitEvents.Domain.Events.AutorEvents;
using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = ["Autor"])]
public sealed class Autor : Entity
{
    [JsonConstructor]
    private Autor(Guid id, string nome, string sobre, string biografia, string? imagem, DateTime dataCriacao, DateTime? dataAtualizacao)
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
    public List<Livro>? Livros { get; private set; }


    public static Autor Create(string nome, string sobre, string biografia)
    {
        var newId = Guid.NewGuid();

        var autor = new Autor(
            id: newId,

            nome: nome.Trim(),
            sobre: sobre.Trim(),
            biografia: biografia.Trim(),
            imagem: null,

            dataCriacao: DateTime.Now,
            dataAtualizacao: null
        );

        Raise(new AutorCriadoEvent(autor.Id));

        return autor;
    }

    public void Update(string nome, string sobre, string biografia, string? extension)
    {
        Nome = nome.Trim();
        Sobre = sobre.Trim();
        Biografia = biografia.Trim();

        var imageName = string.IsNullOrEmpty(extension) ? string.Empty : $"{Id}.{extension}";

        Imagem = imageName;

        DataAtualizacao = DateTime.Now;

        Raise(new AutorAtualizadoEvent(Id, imageName));
    }


    public void SetImage(string imageName)
    {
        Imagem = imageName;
    }
}
