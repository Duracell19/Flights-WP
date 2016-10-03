using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        public string ClearHelpFlyInformation
        {
            get { return "Clear fields"; }
        }

        public string ChangeHelpFlyInformation
        {
            get { return " Сhange fields"; }
        }

        public string FavoriteHelpFlyInformation
        {
            get { return "Add to favorites"; }
        }

        public string RefreshHelpFlyinformation
        {
            get { return "Refresh favorite list"; }
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
