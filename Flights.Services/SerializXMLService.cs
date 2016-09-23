using Flights.Infrastructure;
using System.IO;
using System.Xml.Serialization;

namespace Flights.Services
{
    public class SerializXMLService<T> : ISerializXMLService<T>
    {
        public string Serializ(T[] value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, value);
            return writer.ToString();
        }
    }
}
