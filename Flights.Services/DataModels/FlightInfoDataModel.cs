namespace Flights.Services.DataModels
{
    public class FlightInfoDataModel
    {
        public Pagination pagination { get; set; }
        public object[] threads { get; set; }
        public Search search { get; set; }
    }

    public class Pagination
    {
        public bool has_next { get; set; }
        public int per_page { get; set; }
        public int page_count { get; set; }
        public int total { get; set; }
        public int page { get; set; }
    }

    public class Search
    {
        public string date { get; set; }
        public To to { get; set; }
        public From from { get; set; }
    }

    public class To
    {
        public string code { get; set; }
        public string station_type { get; set; }
        public string title { get; set; }
        public string popular_title { get; set; }
        public string short_title { get; set; }
        public string transport_type { get; set; }
        public string type { get; set; }
    }

    public class From
    {
        public string code { get; set; }
        public string station_type { get; set; }
        public string title { get; set; }
        public string popular_title { get; set; }
        public string short_title { get; set; }
        public string transport_type { get; set; }
        public string type { get; set; }
    }
}
