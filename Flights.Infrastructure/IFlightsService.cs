using Flights.Models;
using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface IFlightsService
    {
        Task<FlyInfoModel> GetFlight(string from, string to, string date);
    }
}
