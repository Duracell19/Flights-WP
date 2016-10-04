using Flights.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flights.Infrastructure.Interfaces
{
    public interface IFlightsService
    {
        Task<List<FlyInfoModel>> GetFlightAsync(string from, string to, string date);
        Task<List<FlyInfoModel>> ConfigurationOfFlights(string date, List<string> iatasFrom, List<string> iatasTo);
    }
}
