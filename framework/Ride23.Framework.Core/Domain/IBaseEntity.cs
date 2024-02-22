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
    string? CreatedBy { get; }
    DateTime? LastModifiedOn { get; }
    string? LastModifiedBy { get; }
    bool IsDeleted { get; }
    void UpdateIsDeleted(bool isDeleted);
    void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy);
}