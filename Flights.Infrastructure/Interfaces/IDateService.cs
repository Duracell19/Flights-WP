using System;

namespace Flights.Infrastructure.Interfaces
{
    public interface IDateService
    {
        string GetDate(DateTimeOffset date);
        string ConvertDate(DateTimeOffset date);
    }
}
