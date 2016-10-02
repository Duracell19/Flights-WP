using Flights.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Flights.Services
{
    public class JsonConverter : IJsonConverter
    {
        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
