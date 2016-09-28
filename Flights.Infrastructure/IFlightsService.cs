using Flights.Models;
using System.Threading.Tasks;

namespace Flights.Infrastructure
{
    public interface IFlightsService
    {
        Task<FlyInfoModel> GetFlightAsync(string from, string to, string date);
        Task<FlyInfoModel[]> ConfigurationOfFlights(DataOfFilghtsModel dataOfFlightsModel, string date, bool returnWay);
    }
}
