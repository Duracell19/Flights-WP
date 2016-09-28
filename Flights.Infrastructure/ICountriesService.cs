using System.Collections.Generic;

namespace Flights.Infrastructure
{
    public interface ICountriesService
    {
        List<string> GetCountries();
    }
}
