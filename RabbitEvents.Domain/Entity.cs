using RabbitEvents.Domain.Interfaces.Events;
using Redis.OM.Modeling;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RabbitEvents.Domain;

public abstract class Entity
{
    protected static List<IDomainEvent> _domainEvents = new();

    [RedisIdField]
    [Indexed]
    public Guid Id { get; protected set; }

    public DateTime DataCriacao { get; protected set; }

    public DateTime? DataAtualizacao { get; protected set; }


    [JsonIgnore]
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get => _domainEvents; }

    protected static void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearEvents() => _domainEvents.Clear();
}
