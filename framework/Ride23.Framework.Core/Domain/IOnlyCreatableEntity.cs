namespace Ride23.Framework.Core.Domain;

public interface IOnlyCreatableEntity
{
    DateTime CreatedOn { get; }
    string? CreatedBy { get; }
}