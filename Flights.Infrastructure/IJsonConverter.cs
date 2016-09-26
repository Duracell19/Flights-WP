using System.IO;

namespace Flights.Infrastructure
{
    public interface IJsonConverter
    {
        T Deserialize<T>(string str);
        T Deserialize<T>(Stream stream);
        string Serialize(object obj);
    }
}
