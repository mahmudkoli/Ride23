using Ride23.Framework.Core.Events;

namespace Ride23.Framework.Core.Domain;

public interface IBaseEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent @event);
    IDomainEvent[] ClearDomainEvents();
}

public interface IBaseEntity<TId> : IBaseEntity
{
    TId Id { get; }
}