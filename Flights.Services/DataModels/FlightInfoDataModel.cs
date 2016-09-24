using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flights.Services.DataModels
{
    public class FlightInfoDataModel
    {
        [JsonProperty(PropertyName = "threads")]
        public List<ThreadsInfo> Threads { get; set; }
    }

    public class ThreadsInfo
    {
        [JsonProperty(PropertyName = "arrival")]
        public string Arrival { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }
        [JsonProperty(PropertyName = "arrival_terminal")]
        public string ArrivalTerminal { get; set; }
        [JsonProperty(PropertyName = "from")]
        public FromInfo From { get; set; }
        [JsonProperty(PropertyName = "thread")]
        public ThreadInfo Thread { get; set; }
        [JsonProperty(PropertyName = "departure")]
        public string Departure { get; set; }
        [JsonProperty(PropertyName = "to")]
        public ToInfo To { get; set; }
    }

    public class FromInfo
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

    public class ThreadInfo
    {
        [JsonProperty(PropertyName = "carrier")]
        public CarrierInfo Carrier { get; set; }
        [JsonProperty(PropertyName = "vehicle")]
        public string Vehicle { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }
    }

    public class CarrierInfo
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

    public class ToInfo
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
