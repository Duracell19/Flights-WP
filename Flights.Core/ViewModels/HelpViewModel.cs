using Flights.Infrastructure;
using MvvmCross.Core.ViewModels;

namespace Flights.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        public string ClearHelpFlyInformation
        {
            get { return Decription.CLEAR_HELP_INFORMATION; }
        }

        public string FavoriteHelpFlyInformation
        {
            get { return Decription.FAVORITE_HELP_INFORMATION; }
        }

        public string RefreshHelpFlyinformation
        {
            get { return Decription.REFRESH_HELP_INFORMATION; }
        }
    }
}
