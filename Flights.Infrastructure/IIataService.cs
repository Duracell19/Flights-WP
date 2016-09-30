using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface IIataService
    {
        Task<List<string>> GetIata(string city);
    }
}
