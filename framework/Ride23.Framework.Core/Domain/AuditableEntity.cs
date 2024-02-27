using MassTransit;
using System.Text.Json.Serialization;

namespace Ride23.Framework.Core.Domain;

public abstract class AuditableEntity : AuditableEntity<DefaultIdType>
{
    protected AuditableEntity() => Id = NewId.Next().ToGuid();
}

public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity, ISoftDelete
{
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CreatedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; } = DateTime.UtcNow;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastModifiedBy { get; private set; }
    [JsonIgnore]
    public bool IsDeleted { get; private set; }
    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
}