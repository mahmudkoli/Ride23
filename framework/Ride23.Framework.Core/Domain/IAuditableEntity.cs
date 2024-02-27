namespace Ride23.Framework.Core.Domain;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedOn { get; }
    string? LastModifiedBy { get; }
    void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy);
}