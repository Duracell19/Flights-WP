namespace Flights.Infrastructure
{
    public interface IDeserializXMLService<T>
    {
        T[] Deserializ(T value);
    }
}
