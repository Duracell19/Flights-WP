using MvvmCross.Core.ViewModels;

namespace Flights.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        public string ClearHelpFlyInformation
        {
            get { return "Clear fields"; }
        }

        public string FavoriteHelpFlyInformation
        {
            get { return "Add to favorites"; }
        }

        public string RefreshHelpFlyinformation
        {
            get { return "Refresh favorite list"; }
        }
    }
}
