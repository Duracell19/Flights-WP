using Flights.Infrastructure;
using Newtonsoft.Json;

namespace Flights.Services
{
    public class JsonConverter : IJsonConverter
    {
        public T Deserialize<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response);
        }
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
