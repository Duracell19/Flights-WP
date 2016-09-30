using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface ICitiesService
    {
        Task<List<string>> GetCities(string country);
    }
}
