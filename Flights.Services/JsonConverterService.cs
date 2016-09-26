using Flights.Infrastructure;
using Newtonsoft.Json;

namespace Flights.Services
{
    public class JsonConverterService : IJsonConverterService
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
