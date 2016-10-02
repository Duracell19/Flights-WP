using Flights.Infrastructure.Interfaces;
using Flights.Services.DataModels;
using System.Collections.Generic;
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

        public async Task<List<string>> GetCities(string country)  
        {
            string uri = "http://flybaseapi.azurewebsites.net/odata/country('" + country + "')";
            string response = await _httpService.GetRequest(uri);
            if (response != null)
            {
                AirportInfoDataModel airportInfo = _jsonConverter.Deserialize<AirportInfoDataModel>(response);
                List<string> cities = new List<string>(); 
                foreach (var item in airportInfo.value)
                {
                    cities.Add(item.City); 
                }
                cities.Sort();
                return cities.Distinct().ToList(); 
            }
            return null;
        }
    }
}
