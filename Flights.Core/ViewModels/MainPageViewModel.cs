using Flights.Infrastructure;
using Flights.Infrastructure.Interfaces;
using Flights.Models;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Flights.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        private readonly ICountriesService _countriesService;
        private readonly ICitiesService _citiesService;
        private readonly IIataService _iataService;
        private readonly IHttpService _httpService;
        private readonly IJsonConverter _jsonConverter;
        private readonly IFileStore _fileStore;
        private MainPagePropetiesModel _properties;
        private MainPageCommandsModel _commands;
        private DataOfFlightsModel _dataOfFlightsModel;

        public MainPagePropetiesModel Properties
        {
            get { return _properties; }
        }

        public MainPageCommandsModel Commands
        {
            get { return _commands; }
        }

        public MainPageViewModel(ICountriesService countriesService,
            ICitiesService citiesService,
            IIataService iataService,
            IHttpService httpService,
            IJsonConverter jsonConverter,
            IFileStore fileStore)
        {
            _countriesService = countriesService;
            _citiesService = citiesService;
            _iataService = iataService;
            _httpService = httpService;
            _jsonConverter = jsonConverter;
            _fileStore = fileStore;

            _dataOfFlightsModel = new DataOfFlightsModel();
            _commands = new MainPageCommandsModel();
            _properties = new MainPagePropetiesModel();

            _commands.ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            _commands.SetOneWayCommand = new MvxCommand(SetOneWay);
            _commands.SetReturnWayCommand = new MvxCommand(SetReturnWay);
            _commands.FindFlightsCommand = new MvxCommand(FindFlights);
            _commands.ClearFieldsCommand = new MvxCommand(ClearFields);
            _commands.UpdateFavoriteListCommand = new MvxCommand(UpdateFavoriteList);
            _commands.SetTheVisibilityIconCommand = new MvxCommand(SetTheVisibilityIcon);
            _commands.SelectCountryFromCommand = new MvxCommand(SelectCountryFromAsync);
            _commands.SelectCountryToCommand = new MvxCommand(SelectCountryToAsync);
            _commands.SelectCityFromCommand = new MvxCommand(SelectCityFromAsync);
            _commands.SelectCityToCommand = new MvxCommand(SelectCityToAsync);
            _commands.SetFlightCommand = new MvxCommand<object>(SetFlight);
        }

        public void Init()
        {
            _properties = SetMainPageProperties();
            _properties.FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
        }

        public override void Start()
        {
            base.Start();
        }

        private void SetOneWay()
        {
            _properties.IsCheckedOneWay = true;
            _properties.IsCheckedReturn = false;
            _properties.IsEnabledDateReturn = false;
            _dataOfFlightsModel.ReturnWay = false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetReturnWay()
        {
            _properties.IsCheckedOneWay = false;
            _properties.IsCheckedReturn = true;
            _properties.IsEnabledDateReturn = true;
            _dataOfFlightsModel.ReturnWay = true;
            RaisePropertyChanged(() => Properties);
        }

        private void FindFlights()
        {
            _dataOfFlightsModel.DateOneWay = _properties.DateOneWay.ToString("yyyy-MM-dd");
            _dataOfFlightsModel.DateReturn = _properties.DateReturn.ToString("yyyy-MM-dd");
            var param = _jsonConverter.Serialize(_dataOfFlightsModel);
            ShowViewModel<FlightsListViewModel>(new { param });
        }

        private void ClearFields()
        {
            _properties.TextCountryFrom = null;
            _properties.TextCountryTo = null;
            _properties.TextCityFrom = null;
            _properties.TextCityTo = null;
            _properties.IsCheckedOneWay = true;
            _properties.IsCheckedReturn = false;
            _properties.IsEnabledDateReturn = false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetTheVisibilityIcon()
        {
            if (_properties.PivotNumber == 0)
            {
                _properties.VisibilityRefresh = false;
                _properties.VisibilityClear = true;
            }
            else
            {
                _properties.VisibilityRefresh = (_properties.PivotNumber == 1) ? true : false;
                _properties.VisibilityClear = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private void UpdateFavoriteList()
        {
            _properties.FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            RaisePropertyChanged(() => Properties);
        }

        public async void SelectCountryFromAsync()
        {
            _properties.CitiesFrom = await _citiesService.GetCitiesAsync(_properties.TextCountryFrom);
            if (_properties.CitiesFrom != null)
            {
                _properties.IsEnabledCityFrom = true;
                _properties.PlaceholderTextCityFrom = "Choose city";
                _dataOfFlightsModel.CountryFrom = _properties.TextCountryFrom;
                _dataOfFlightsModel.CitiesFrom = _properties.CitiesFrom;
            }
            else
            {
                _properties.PlaceholderTextCityFrom = "No available airports";
                _properties.IsEnabledCityFrom = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCountryToAsync()
        {
            _properties.CitiesTo = await _citiesService.GetCitiesAsync(_properties.TextCountryTo);
            if (_properties.CitiesTo != null)
            {
                _properties.IsEnabledCityTo = true;
                _properties.PlaceholderTextCityTo = "Choose city";
                _dataOfFlightsModel.CountryTo = _properties.TextCountryTo;
                _dataOfFlightsModel.CitiesTo = _properties.CitiesTo;
            }
            else
            {
                _properties.PlaceholderTextCityTo = "No available airports";
                _properties.IsEnabledCityTo = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityFromAsync()
        {
            _dataOfFlightsModel.CityFrom = _properties.TextCityFrom;
            _dataOfFlightsModel.IatasFrom = await _iataService.GetIataAsync(_properties.TextCityFrom);
            _properties.IsEnabledButtonFind = (IsDataAboutFlightExist()) ? true : false;
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityToAsync()
        {
            _dataOfFlightsModel.CityTo = _properties.TextCityTo;
            _dataOfFlightsModel.IatasTo = await _iataService.GetIataAsync(_properties.TextCityTo);
            _properties.IsEnabledButtonFind = (IsDataAboutFlightExist()) ? true : false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetFlight(object arg)
        {
            if (arg is FavoriteModel)
            {
                ClearFields();
                var item = (FavoriteModel)arg;
                _properties.TextCountryFrom = item.CountryFrom;
                _properties.TextCountryTo = item.CountryTo;
                _properties.TextCityFrom = item.CityFrom;
                _properties.TextCityTo = item.CityTo;
                _properties.PivotNumber = 0;
                RaisePropertyChanged(() => Properties);
            }
        }

        private MainPagePropetiesModel SetMainPageProperties()
        {
            return new MainPagePropetiesModel
            {
                TextAbout = Decription.ABOUT_INFORMATION,
                PlaceholderTextCityFrom = "Choose city",
                PlaceholderTextCityTo = "Choose city",
                DateOneWay = DateTime.Now,
                DateReturn = DateTime.Now,
                IsEnabledButtonFind = false,
                IsEnabledCityFrom = false,
                IsEnabledCityTo = false,
                IsCheckedOneWay = true,
                IsEnabledDateReturn = false,
                CountriesFrom = _countriesService.GetCountries(),
                CountriesTo = _countriesService.GetCountries()
            };
        }

        private bool IsDataAboutFlightExist()
        {
            return _properties.TextCityFrom != null && _properties.TextCityTo != null &&
                _dataOfFlightsModel.IatasTo != null && _dataOfFlightsModel.IatasFrom != null;
        }
    }
}

