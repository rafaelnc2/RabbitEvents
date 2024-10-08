using Redis.OM.Modeling;

namespace RabbitEvents.Domain.ValueObjects;

public class AuthorInfoVo
{
    public AuthorInfoVo(string id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    [Indexed]
    public string Id { get; init; }

    [Searchable]
    public string Nome { get; init; } = null!;
}
