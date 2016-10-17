using Flights.Infrastructure.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class HttpService : IHttpService
    {
        public async Task<string> GetRequestAsync(string url)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri(url);
                var response = await client.GetStringAsync(uri);
                client.Dispose();
                return response;
            }
            catch(HttpRequestException)
            {
                return null;
            }
        }
    }
}
