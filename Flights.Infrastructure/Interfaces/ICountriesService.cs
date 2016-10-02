using System.Collections.Generic;

namespace Flights.Infrastructure.Interfaces
{
    public interface ICountriesService
    {
        List<string> GetCountries();
    }
}
