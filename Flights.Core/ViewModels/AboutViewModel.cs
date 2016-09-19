using Flights.Infrastructure;
using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        readonly IWPHardwareButtonEvents _platformEvents;

        public AboutViewModel(IWPHardwareButtonEvents platformEvents)
        {
            _platformEvents = platformEvents;
            _platformEvents.BackButtonPressed += BackButtonPressed;
        }

        void BackButtonPressed(object sender, EventArgs e)
        {
            Close(this);
            _platformEvents.BackButtonPressed -= BackButtonPressed;
        }

        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
    }
}
