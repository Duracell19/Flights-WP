using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class CarrierInfoDataModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
