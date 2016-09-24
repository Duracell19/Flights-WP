namespace Flights.Infrastructure
{
    public interface IJsonConverter
    {
        T Deserialize<T>(string response);
        string Serialize<T>(T obj);
    }
}
