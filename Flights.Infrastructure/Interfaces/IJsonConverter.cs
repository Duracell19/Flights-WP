namespace Flights.Infrastructure.Interfaces
{
    public interface IJsonConverter
    {
        T Deserialize<T>(string str);
        string Serialize(object obj);
    }
}
