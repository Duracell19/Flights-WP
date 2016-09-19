using System;

namespace Flights.Infrastructure
{
    public interface IWPHardwareButtonEvents
    {
        event EventHandler BackButtonPressed;
    }
}
