using Redis.OM.Modeling;

namespace RabbitEvents.Domain.ValueObjects;

public class AuthorInfoVo
{
    public AuthorInfoVo(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    [Indexed]
    public Guid Id { get; init; }

    [Searchable]
    public string Nome { get; init; } = null!;
}
