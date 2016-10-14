using System.Threading.Tasks;

namespace Flights.Infrastructure.Interfaces
{
    public interface IHttpService
    {
        Task<string> GetRequestAsync(string url);
    }
}
