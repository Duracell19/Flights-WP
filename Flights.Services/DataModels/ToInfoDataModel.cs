using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class ToInfoDataModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
