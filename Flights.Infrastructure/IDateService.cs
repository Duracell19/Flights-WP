using System;

namespace Flights.Infrastructure
{
    public interface IDateService
    {
        string GetDate(DateTimeOffset date);
        string ConvertDate(DateTimeOffset date);
    }
}
