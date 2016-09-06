using Flights.Infrastructure;
using System.IO;
using System.Xml.Serialization;

namespace Flights.Services.DataModels
{
    public class SerializService : ISerializService
    {
        public string Serializ(string[] value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, value);
            return writer.ToString();
        }
    }
}
