using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface IHttpService
    {
        Task<string> GetRequest(string url);
    }
}
