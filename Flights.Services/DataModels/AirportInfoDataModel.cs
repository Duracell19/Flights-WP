using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flights.Services.DataModels
{
    public class AirportInfoDataModel
    {
        [JsonProperty(PropertyName = "value")]
        public List<ValueDataModel> value { get; set; }
    }
}
