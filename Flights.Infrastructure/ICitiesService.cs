using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface ICitiesService
    {
        Task<string[]> GetCities(string country);
    }
}
