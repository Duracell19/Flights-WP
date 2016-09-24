using Flights.Infrastructure;
using Flights.Services.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class CitiesService : ICitiesService
    {
        readonly IHttpService _httpService;
        readonly IJsonConverter _jsonConverter;

        public CitiesService(IHttpService httpService, IJsonConverter jsonConverter)
        {
            _httpService = httpService;
            _jsonConverter = jsonConverter;
        }

        public async Task<string[]> GetCities(string country)
        {
            string uri = "http://flybaseapi.azurewebsites.net/odata/country('" + country + "')";
            string response = await _httpService.GetRequest(uri);
            if (response != null)
            {
                AirportInfoDataModel airportInfo = _jsonConverter.Deserialize<AirportInfoDataModel>(response);
                string[] cities = new string[airportInfo.value.Count];
                int i = 0;
                foreach (var item in airportInfo.value)
                {
                    cities[i] = item.City;
                    i++;
                }
                Array.Sort(cities);
                return cities.Distinct().ToArray();
            }
            return null;
        }
    }
}
