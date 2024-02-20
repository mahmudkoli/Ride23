using Ride23.Framework.Core.Services;

namespace Ride23.Framework.Core.Serializers;

public interface ISerializerService : ITransientService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}
