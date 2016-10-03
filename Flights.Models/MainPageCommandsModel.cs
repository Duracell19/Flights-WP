using System.Windows.Input;

namespace Flights.Models
{
    public class MainPageCommandsModel
    {
        public ICommand SelectCountryFromCommand { get; set; }
        public ICommand SelectCountryToCommand { get; set; }
        public ICommand SelectCityFromCommand { get; set; }
        public ICommand SelectCityToCommand { get; set; }
        public ICommand SetOneWayCommand { get; set; }
        public ICommand SetReturnWayCommand { get; set; }
        public ICommand FindFlightsCommand { get; set; }
        public ICommand ChangeFieldsCommand { get; set; }
        public ICommand ClearFieldsCommand { get; set; }
        public ICommand SetTheVisibilityIconCommand { get; set; }
        public ICommand ShowHelpInformationCommand { get; set; }
        public ICommand SetFlightCommand { get; set; }
        public ICommand DeleteFavoriteFlightCommand { get; set; }
        public ICommand UpdateFavoriteListCommand { get; set; }
    }
}
