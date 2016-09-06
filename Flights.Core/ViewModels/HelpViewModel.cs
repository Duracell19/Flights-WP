using MvvmCross.Core.ViewModels;
using System.Windows.Input;
namespace Flights.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    { 
        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
        public string ClearText
        {
            get
            {
                return "Input fields are cleared when you click on this item";
            }
        }
        public string ChangeText
        {
            get
            {
                return "Input fields are reversed when you click on this icon";
            }
        }
        public string FavoriteText
        {
            get
            {
                return "Flights are added to favorites by clicking on the icon for further use";
            }
        }
    }
}
