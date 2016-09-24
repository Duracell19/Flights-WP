using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flights.Services.DataModels
{
    public class AirportInfoDataModel
    {
        [JsonProperty(PropertyName = "value")]
        public List<Value> value { get; set; }
    }

    public class Value
    {
        [JsonProperty(PropertyName = "Iata")]
        public string Iata { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
    }
}
