using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flights.Infrastructure.Interfaces
{
    public interface ICitiesService
    {
        Task<List<string>> GetCitiesAsync(string country);
    }
}
