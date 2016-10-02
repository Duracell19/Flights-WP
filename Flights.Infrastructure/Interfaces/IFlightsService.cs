using Flights.Models;
using System.Threading.Tasks;

namespace Flights.Infrastructure.Interfaces
{
    public interface IFlightsService
    {
        Task<FlyInfoModel> GetFlightAsync(string from, string to, string date);
        Task<FlyInfoModel[]> ConfigurationOfFlights(DataOfFlightsModel dataOfFlightsModel, string date, bool returnWay);
    }
}
