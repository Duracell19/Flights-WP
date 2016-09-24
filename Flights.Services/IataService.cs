using Flights.Infrastructure;
using Flights.Services.DataModels;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class IataService : IIataService
    {
        readonly IHttpService _httpService;
        readonly IJsonConverter _jsonConverter;

        public IataService(IHttpService httpService, IJsonConverter jsonConverter)
        {
            _httpService = httpService;
            _jsonConverter = jsonConverter;
        }

        public async Task<string[]> GetIata(string city)
        {
            string uri = "http://flybaseapi.azurewebsites.net/odata/code_iata('" + city + "')";
            string response = await _httpService.GetRequest(uri);
            if (response != null)
            {
                AirportInfoDataModel airportInfo = _jsonConverter.Deserialize<AirportInfoDataModel>(response);
                string[] iata = new string[airportInfo.value.Count];
                int i = 0;
                foreach (var item in airportInfo.value)
                {
                    iata[i] = item.Iata;
                    i++;
                }
                return iata;
            }
            return null;
        }
    }
}
