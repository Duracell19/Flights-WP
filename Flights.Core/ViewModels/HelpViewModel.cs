using Flights.Infrastructure;
using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;
namespace Flights.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        readonly IWPHardwareButtonEvents _platformEvents;

        public HelpViewModel(IWPHardwareButtonEvents platformEvents)
        {
            _platformEvents = platformEvents;
            _platformEvents.BackButtonPressed += BackButtonPressed;
        }

        public string ClearHelpFlyInformation
        {
            get
            {
                return "Clear fields";
            }
        }

        public string ChangeHelpFlyInformation
        {
            get
            {
                return " Сhange fields";
            }
        }

        public string FavoriteHelpFlyInformation
        {
            get
            {
                return "Add to favorites";
            }
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
