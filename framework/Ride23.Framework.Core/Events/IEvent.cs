using MediatR;

namespace Ride23.Framework.Core.Events;
public interface IEvent : INotification
{
    DefaultIdType Id { get; }
    DateTime CreationDate { get; }
    IDictionary<string, object> MetaData { get; }
}
