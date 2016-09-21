using Flights.Infrastructure;
using System.IO;
using System.Xml.Serialization;

namespace Flights.Services
{
    public class DeserializService : IDeserializService
    {
        public string[] Deserializ(string value)
        {
            string[] result;
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            using (TextReader reader = new StringReader(value))
            {
                result = (string[])serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
