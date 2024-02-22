namespace Ride23.Framework.Core.Events;
public interface IEvent
{
    DefaultIdType Id { get; }
    DateTime CreationDate { get; }
    IDictionary<string, object> MetaData { get; }
}
