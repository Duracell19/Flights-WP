using Flights.Infrastructure;
using Flights.Models;
using Flights.Services.DataModels;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class FlightsService: IFlightsService
    {
        readonly IHttpService _httpService;
        readonly IJsonConverter _jsonConverter;

        public FlightsService(IHttpService httpService, IJsonConverter jsonConverter, IHttpService httpService, IJsonConverter jsonConverter) 
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
                flyInfoModel.Arrival = new string[flightInfoModel.Threads.Count];
                flyInfoModel.Duration = new string[flightInfoModel.Threads.Count];
                flyInfoModel.ArrivalTerminal = new string[flightInfoModel.Threads.Count];
                flyInfoModel.From = new string[flightInfoModel.Threads.Count];
                flyInfoModel.ThreadCarrierTitle = new string[flightInfoModel.Threads.Count];
                flyInfoModel.ThreadVehicle = new string[flightInfoModel.Threads.Count];
                flyInfoModel.ThreadNumber = new string[flightInfoModel.Threads.Count];
                flyInfoModel.Departure = new string[flightInfoModel.Threads.Count];
                flyInfoModel.To = new string[flightInfoModel.Threads.Count];
                foreach (var item in flightInfoModel.Threads)
                {
                    flyInfoModel.Arrival[i] = item.Arrival;
                    flyInfoModel.Duration[i] = item.Duration;
                    flyInfoModel.ArrivalTerminal[i] = item.ArrivalTerminal;
                    flyInfoModel.From[i] = item.From.Title;
                    flyInfoModel.ThreadCarrierTitle[i] = item.Thread.Carrier.Title;
                    flyInfoModel.ThreadVehicle[i] = item.Thread.Vehicle;
                    flyInfoModel.ThreadNumber[i] = item.Thread.Number;
                    flyInfoModel.Departure[i] = item.Departure;
                    flyInfoModel.To[i] = item.To.Title;
                    i++;
                }
                return flyInfoModel;
            }
            return null;

            //if (response == null)
            //    return null;
            //dynamic answer = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            //FlyInfoModel flyInfoModel = new FlyInfoModel();
            //flyInfoModel.Count = answer.threads.Count;
            //if (flyInfoModel.Count != 0)
            //{
            //    flyInfoModel.Arrival = new string[flyInfoModel.Count];
            //    flyInfoModel.Duration = new string[flyInfoModel.Count];
            //    flyInfoModel.ArrivalTerminal = new string[flyInfoModel.Count];
            //    flyInfoModel.From = new string[flyInfoModel.Count];
            //    flyInfoModel.ThreadCarrierTitle = new string[flyInfoModel.Count];
            //    flyInfoModel.ThreadVehicle = new string[flyInfoModel.Count];
            //    flyInfoModel.ThreadNumber = new string[flyInfoModel.Count];
            //    flyInfoModel.Departure = new string[flyInfoModel.Count];
            //    flyInfoModel.To = new string[flyInfoModel.Count];
            //    for (int i = 0; i < flyInfoModel.Count; i++)
            //    {
            //        flyInfoModel.Arrival[i] = answer.threads[i].arrival;
            //        flyInfoModel.Duration[i] = answer.threads[i].duration;
            //        flyInfoModel.ArrivalTerminal[i] = answer.threads[i].arrival_terminal;
            //        flyInfoModel.From[i] = answer.threads[i].from.title;
            //        flyInfoModel.ThreadCarrierTitle[i] = answer.threads[i].thread.carrier.title;
            //        flyInfoModel.ThreadVehicle[i] = answer.threads[i].thread.vehicle;
            //        flyInfoModel.ThreadNumber[i] = answer.threads[i].thread.number;
            //        flyInfoModel.Departure[i] = answer.threads[i].departure;
            //        flyInfoModel.To[i] = answer.threads[i].to.title;
            //    }
            //    return flyInfoModel;
            //}
            //return null;
        }
    }
}
