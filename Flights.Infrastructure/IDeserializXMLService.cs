namespace Flights.Infrastructure
{
    public interface IDeserializXMLService
    {
        T[] Deserializ<T>(T value);
    }
}
