using Flights.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class CitiesService : ICitiesService
    {
        readonly IHttpService _httpService;

        public CitiesService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public  async Task<string[]> GetCities(string country)
        {
            string[] cities;
            string uri = "http://flybaseapi.azurewebsites.net/odata/country('" + country + "')";
            string response = await _httpService.GetRequest(uri);
            if (response == null)
                return null;
            else
            {
                dynamic answer = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                int count = answer.value.Count;
                cities = new string[count];
                for (int i = 0; i < count; i++)
                {
                    cities[i] = Convert.ToString(answer.value[i]);
                    dynamic input_answer = Newtonsoft.Json.JsonConvert.DeserializeObject(cities[i]);
                    cities[i] = Convert.ToString(input_answer.City);
                }
                Array.Sort(cities);
                string[] result = cities.Distinct().ToArray();
                return result;
            }
        }
    }
}
