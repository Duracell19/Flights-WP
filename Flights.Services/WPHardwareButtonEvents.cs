using Flights.Infrastructure;
using System;
using Windows.Phone.UI.Input;

namespace Flights.Services
{
    public class WPHardwareButtonEvents : IWPHardwareButtonEvents
    {
        public event EventHandler BackButtonPressed;

        public WPHardwareButtonEvents()
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (BackButtonPressed != null)
            {
                BackButtonPressed(this, new EventArgs());
                e.Handled = true;
            }
        }
    }
}
