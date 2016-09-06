using Flights.Infrastructure;
using System;

namespace Flights.Services.DataModels
{
    public class DateService : IDateService
    {
        public string GetDate(DateTimeOffset date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }
        public string ConvertDate(DateTimeOffset date)
        {
            return date.Month + "/" + date.Day + "/" + date.Year;
        }
    }
}
