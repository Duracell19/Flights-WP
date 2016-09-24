using Flights.Infrastructure;
using Flights.Models;
using Flights.Services.DataModels;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class FlightsService: IFlightsService
    {
        readonly IHttpService _httpService;
        readonly IJsonConverter _jsonConverter;

        public FlightsService(IHttpService httpService, IJsonConverter jsonConverter)
        {
            _httpService = httpService;
            _jsonConverter = jsonConverter; 
        }

        public async Task<FlyInfoModel> GetFlight(string from, string to, string date)
        {
            string uri = "https://api.rasp.yandex.net/v1.0/search/?apikey=e07ef310-dbe4-49cf-985f-1d5738c1ebc7&format=json&transport_types=plane&system=iata&from=" + from + "&to=" + to + "&lang=en&page=1&date=" + date;
            string response = await _httpService.GetRequest(uri);
            if (response != null)
            {
                FlightInfoDataModel flightInfoModel = _jsonConverter.Deserialize<FlightInfoDataModel>(response);
                FlyInfoModel flyInfoModel = new FlyInfoModel();
                int i = 0;
                foreach (var item in flightInfoModel.Threads)
                {
                    flyInfoModel.Arrival[i] = item.Arrival;
                    flyInfoModel.Duration[i] = item.Duration;
                    flyInfoModel.ArrivalTerminal[i] = item.ArrivalTerminal;
                    flyInfoModel.From[i] = item.From.First().Title;
                    flyInfoModel.ThreadCarrierTitle[i] = item.Thread.First().Carrier.First().Title;
                    flyInfoModel.ThreadVehicle[i] = item.Thread.First().Vehicle;
                    flyInfoModel.ThreadNumber[i] = item.Thread.First().Number;
                    flyInfoModel.Departure[i] = item.Departure;
                    flyInfoModel.To[i] = item.To.First().Title;
                    i++;
                }
                return flyInfoModel;
            }
            return null;
        }
    }
}
