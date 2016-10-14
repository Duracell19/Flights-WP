using Flights.Infrastructure.Interfaces;
using Flights.Services.DataModels;
using System.Collections.Generic;
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

        public async Task<List<string>> GetIataAsync(string city) 
        {
            var uri = "http://flybaseapi.azurewebsites.net/odata/code_iata('" + city + "')";
            var response = await _httpService.GetRequestAsync(uri);
            if (response != null)
            {
                var airportInfo = _jsonConverter.Deserialize<AirportInfoDataModel>(response);
                var iata = new List<string>(); 
                foreach (var item in airportInfo.value)
                {
                    iata.Add(item.Iata);
                }
                return iata;
            }
            return null;
        }
    }
}
