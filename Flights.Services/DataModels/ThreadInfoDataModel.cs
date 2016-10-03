using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class ThreadInfoDataModel
    {
        [JsonProperty(PropertyName = "carrier")]
        public CarrierInfoDataModel Carrier { get; set; }
        [JsonProperty(PropertyName = "vehicle")]
        public string Vehicle { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }
    }
}
