using Flights.Infrastructure.Interfaces;
using Flights.Models;
using Flights.Services.DataModels;
using System.Collections.Generic;
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

        public async Task<List<FlyInfoModel>> GetFlightAsync(string date, string from, string to)
        {
            var uri = "https://api.rasp.yandex.net/v1.0/search/?apikey=e07ef310-dbe4-49cf-985f-1d5738c1ebc7&format=json&transport_types=plane&system=iata&from=" + from + "&to=" + to + "&lang=en&page=1&date=" + date;
            var response = await _httpService.GetRequestAsync(uri);
            if (response == null)
            {
                return null;
            }
            var flightInfoModel = _jsonConverter.Deserialize<FlightInfoDataModel>(response);
            var flyInfoModel = new List<FlyInfoModel>();
            flyInfoModel.AddRange(flightInfoModel.Threads.Select(CreateFlyInfoModel));
            return flyInfoModel;
        }

        private FlyInfoModel CreateFlyInfoModel(ThreadsInfoDataModel model)
        {
            return new FlyInfoModel
            {
                Arrival = model.Arrival,
                Duration = model.Duration,
                ArrivalTerminal = model.ArrivalTerminal,
                From = model.From.Title,
                ThreadCarrierTitle = model.Thread.Carrier.Title,
                ThreadVehicle = model.Thread.Vehicle,
                ThreadNumber = model.Thread.Number,
                Departure = model.Departure,
                To = model.To.Title
            };
        }

        public async Task<List<FlyInfoModel>> ConfigurationOfFlightsAsync(string date, List<string> iatasFrom, List<string> iatasTo)
        {
            var flyInfoModel = new List<FlyInfoModel>();
            foreach (var iataFrom in iatasFrom)
            {
                foreach (var iataTo in iatasTo)
                {
                    var result = await GetFlightAsync(date, iataFrom, iataTo);
                    if (result == null)
                    {
                        continue;
                    }
                    flyInfoModel.AddRange(result);
                }
            }
            return flyInfoModel;
        }
    }
}
