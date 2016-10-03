using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class FromInfoDataModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
