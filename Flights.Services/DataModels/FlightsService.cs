using Flights.Infrastructure;
using Flights.Models;
using System.Threading.Tasks;

namespace Flights.Services.DataModels
{
    public class FlightsService: IFlightsService
    {
        readonly IHttpService _httpService;

        public FlightsService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<FlyInfoModel> GetFlight(string from, string to, string date)
        {
            string uri = "https://api.rasp.yandex.net/v1.0/search/?apikey=e07ef310-dbe4-49cf-985f-1d5738c1ebc7&format=json&transport_types=plane&system=iata&from=" + from + "&to=" + to + "&lang=en&page=1&date=" + date;
            string response = await _httpService.GetRequest(uri);
            if (response == null)
                return null;
            dynamic answer = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            FlyInfoModel flyInfoModel = new FlyInfoModel();
            flyInfoModel.Count = answer.threads.Count;
            if (flyInfoModel.Count != 0)
            {
                flyInfoModel.Arrival = new string[flyInfoModel.Count];
                flyInfoModel.Duration = new string[flyInfoModel.Count];
                flyInfoModel.ArrivalTerminal = new string[flyInfoModel.Count];
                flyInfoModel.FromTitle = new string[flyInfoModel.Count];
                flyInfoModel.ThreadCarrierTitle = new string[flyInfoModel.Count];
                flyInfoModel.ThreadVehicle = new string[flyInfoModel.Count];
                flyInfoModel.ThreadNumber = new string[flyInfoModel.Count];
                flyInfoModel.Departure = new string[flyInfoModel.Count];
                flyInfoModel.ToTitle = new string[flyInfoModel.Count];
                for (int i = 0; i < flyInfoModel.Count; i++)
                {
                    flyInfoModel.Arrival[i] = answer.threads[i].arrival;
                    flyInfoModel.Duration[i] = answer.threads[i].duration;
                    flyInfoModel.ArrivalTerminal[i] = answer.threads[i].arrival_terminal;
                    flyInfoModel.FromTitle[i] = answer.threads[i].from.title;
                    flyInfoModel.ThreadCarrierTitle[i] = answer.threads[i].thread.carrier.title;
                    flyInfoModel.ThreadVehicle[i] = answer.threads[i].thread.vehicle;
                    flyInfoModel.ThreadNumber[i] = answer.threads[i].thread.number;
                    flyInfoModel.Departure[i] = answer.threads[i].departure;
                    flyInfoModel.ToTitle[i] = answer.threads[i].to.title;
                }
                return flyInfoModel;
            }
            return null;
        }
    }
}
