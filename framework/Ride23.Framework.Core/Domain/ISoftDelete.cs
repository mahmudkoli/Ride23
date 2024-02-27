namespace Ride23.Framework.Core.Domain;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    void UpdateIsDeleted(bool isDeleted);
}