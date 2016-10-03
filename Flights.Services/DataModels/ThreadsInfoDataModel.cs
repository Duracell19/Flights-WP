using Newtonsoft.Json;

namespace Flights.Services.DataModels
{
    public class ThreadsInfoDataModel
    {
        [JsonProperty(PropertyName = "arrival")]
        public string Arrival { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }
        [JsonProperty(PropertyName = "arrival_terminal")]
        public string ArrivalTerminal { get; set; }
        [JsonProperty(PropertyName = "from")]
        public FromInfoDataModel From { get; set; }
        [JsonProperty(PropertyName = "thread")]
        public ThreadInfoDataModel Thread { get; set; }
        [JsonProperty(PropertyName = "departure")]
        public string Departure { get; set; }
        [JsonProperty(PropertyName = "to")]
        public ToInfoDataModel To { get; set; }
    }
}
