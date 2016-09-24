using Flights.Infrastructure;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Flights.Services
{
    public class DeserializXMLService : IDeserializXMLService
    {
        public T[] Deserializ<T>(T value)
        {
            T[] result;
            XmlSerializer serializer = new XmlSerializer(typeof(T[]));
            using (TextReader reader = new StringReader(Convert.ToString(value)))
            {
                result = (T[])serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
