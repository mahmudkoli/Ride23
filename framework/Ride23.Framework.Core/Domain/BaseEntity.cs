using Ride23.Framework.Core.Events;
using MassTransit;
using System.Text.Json.Serialization;

namespace Ride23.Framework.Core.Domain;
public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    [JsonPropertyOrder(-1)]
    public TId Id { get; protected set; } = default!;
    [JsonIgnore]
    private readonly List<IDomainEvent> _domainEvents = new();
    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}