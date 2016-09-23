namespace Flights.Infrastructure
{
    public interface ISerializXMLService<T>
    {
        string Serializ(T[] value);
    }
}
