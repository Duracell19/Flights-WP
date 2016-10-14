using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flights.Infrastructure.Interfaces
{
    public interface IIataService
    {
        Task<List<string>> GetIataAsync(string city);
    }
}
