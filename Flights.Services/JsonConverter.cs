using Flights.Infrastructure;
using Newtonsoft.Json;
using System.IO;

namespace Flights.Services
{
    public class JsonConverter : IJsonConverter
    {
        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public T Deserialize<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
