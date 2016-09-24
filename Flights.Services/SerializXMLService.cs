using Flights.Infrastructure;
using System.IO;
using System.Xml.Serialization;

namespace Flights.Services
{
    public class SerializXMLService : ISerializXMLService
    {
        public string Serializ<T>(T[] value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, value);
            return writer.ToString();
        }
    }
}
