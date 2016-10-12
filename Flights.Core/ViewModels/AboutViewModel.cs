using Flights.Infrastructure;
using MvvmCross.Core.ViewModels;

namespace Flights.Core.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        public string TextAbout
        {
            get { return Decription.ABOUT_INFORMATION; }
        }
    }
}
