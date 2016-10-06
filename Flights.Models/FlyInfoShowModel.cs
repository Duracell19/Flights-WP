namespace Flights.Models
{
    public class FlyInfoShowModel
    {
        public string Arrival { get; set; }
        public string Duration { get; set; }
        public string ArrivalTerminal { get; set; }
        public string ThreadCarrierTitle { get; set; }
        public string ThreadVehicle { get; set; }
        public string ThreadNumber { get; set; }
        public string Departure { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public bool IsReservedFlight { get; set; }
    }
}
