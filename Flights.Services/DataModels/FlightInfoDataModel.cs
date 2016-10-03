using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flights.Services.DataModels
{
    public class FlightInfoDataModel
    {
        [JsonProperty(PropertyName = "threads")]
        public List<ThreadsInfoDataModel> Threads { get; set; }
    }
}
