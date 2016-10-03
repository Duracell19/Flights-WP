using System.Collections.Generic;

namespace Flights.Models
{
    public class DataOfFlightsModel
    { 
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public List<string> IataFrom { get; set; }
        public List<string> IataTo { get; set; }
        public List<string> CitiesFrom { get; set; }
        public List<string> CitiesTo { get; set; }
        public bool ReturnWay { get; set; }
        public string DateOneWay { get; set; }
        public string DateReturn { get; set; }
    }
}
