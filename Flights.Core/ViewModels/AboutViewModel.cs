using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
    }
}
