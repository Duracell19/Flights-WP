using Flights.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Flights.Services.DataModels
{
    public class IataService : IIataService
    {
        readonly IHttpService _httpService;
        public IataService(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<string[]> GetIata(string city)
        {
            string[] iata;
            string uri = "http://flybaseapi.azurewebsites.net/odata/code_iata('" + city + "')";
            string response = await _httpService.GetRequest(uri);
            if (response == null)
                return null;
            dynamic answer = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            int count = answer.value.Count;
            iata = new string[count];
            for (int i = 0; i < count; i++)
            {
                iata[i] = Convert.ToString(answer.value[i]);
                dynamic input_answer = Newtonsoft.Json.JsonConvert.DeserializeObject(iata[i]);
                iata[i] = Convert.ToString(input_answer.Iata);
            }
            return iata;
        }
    }
}
