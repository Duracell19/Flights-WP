using Flights.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flights.Services.DataModels
{
    public class HttpService : IHttpService
    {
        public async Task<string> GetRequest(string url)
        {
            try
            {
                var client = new HttpClient();
                Uri uri = new Uri(url);
                string response = await client.GetStringAsync(uri);
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
