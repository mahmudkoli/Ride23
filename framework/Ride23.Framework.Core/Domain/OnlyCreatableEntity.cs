using MassTransit;
using System.Text.Json.Serialization;

namespace Ride23.Framework.Core.Domain;

public abstract class OnlyCreatableEntity : OnlyCreatableEntity<DefaultIdType>
{
    protected OnlyCreatableEntity() => Id = NewId.Next().ToGuid();
}

public abstract class OnlyCreatableEntity<TId> : BaseEntity<TId>, IOnlyCreatableEntity
{
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CreatedBy { get; private set; }
}