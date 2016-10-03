using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class ValueDataModel
    {
        [JsonProperty(PropertyName = "Iata")]
        public string Iata { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
    }
}
