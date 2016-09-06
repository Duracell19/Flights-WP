using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface IIataService
    {
        Task<string[]> GetIata(string city);
    }
}
