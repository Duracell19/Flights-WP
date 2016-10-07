using Flights.Infrastructure;
using Flights.Infrastructure.Interfaces;
using Flights.Models;
using Flights.Services;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Flights.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        private readonly ICountriesService _countriesService;
        private readonly ICitiesService _citiesService;
        private readonly IHttpService _httpService;
        private readonly IJsonConverter _jsonConverter;
        private readonly IFileStore _fileStore;
        private List<MainPagePropetiesModel> _properties;
        private List<MainPageCommandsModel> _commands;
        private DataOfFlightsModel _dataOfFlightsModel;
        private bool _status;

        public List<MainPagePropetiesModel> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }
        public List<MainPageCommandsModel> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public MainPageViewModel(ICountriesService countriesService,
            ICitiesService citiesService,
            IHttpService httpService,
            IJsonConverter jsonConverter,
            IFileStore fileStore)
        {
            _countriesService = countriesService;
            _citiesService = citiesService;
            _httpService = httpService;
            _jsonConverter = jsonConverter;
            _fileStore = fileStore;

            _dataOfFlightsModel = new DataOfFlightsModel();
            _commands = new List<MainPageCommandsModel>();
            _properties = new List<MainPagePropetiesModel>();

            Properties.Add(new MainPagePropetiesModel
            {
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
                CountriesTo = _countriesService.GetCountries(),
            });
            Commands.Add(new MainPageCommandsModel { });

            Commands[0].ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            Commands[0].SetOneWayCommand = new MvxCommand(SetOneWay);
            Commands[0].SetReturnWayCommand = new MvxCommand(SetReturnWay);
            Commands[0].FindFlightsCommand = new MvxCommand(FindFlights);
            Commands[0].ChangeFieldsCommand = new MvxCommand(ChangeFields);
            Commands[0].ClearFieldsCommand = new MvxCommand(ClearFields);
            Commands[0].UpdateFavoriteListCommand = new MvxCommand(UpdateFavoriteList);
            Commands[0].SetTheVisibilityIconCommand = new MvxCommand(SetTheVisibilityIcon);
            Commands[0].SelectCountryFromCommand = new MvxCommand(SelectCountryFromAsync);
            Commands[0].SelectCountryToCommand = new MvxCommand(SelectCountryToAsync);
            Commands[0].SelectCityFromCommand = new MvxCommand(SelectCityFromAsync);
            Commands[0].SelectCityToCommand = new MvxCommand(SelectCityToAsync);
            Commands[0].SetFlightCommand = new MvxCommand<object>(SetFlight);
        }

        public override void Start()
        {
            base.Start();
        }

        public void Init()
        {
            Properties[0].FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            if (Properties[0].FavoriteList == null)
            {
                Properties[0].FavoriteList = new ObservableCollection<FavoriteModel>();
            }
            RaisePropertyChanged(() => Properties);
        }

        private void SetOneWay()
        {
            Properties[0].IsCheckedOneWay = true;
            Properties[0].IsCheckedReturn = false;
            _dataOfFlightsModel.ReturnWay = false;
            Properties[0].IsEnabledDateReturn = false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetReturnWay()
        {
            Properties[0].IsCheckedOneWay = false;
            Properties[0].IsCheckedReturn = true;
            _dataOfFlightsModel.ReturnWay = true;
            Properties[0].IsEnabledDateReturn = true;
            RaisePropertyChanged(() => Properties);
        }

        private void FindFlights()
        {
            _dataOfFlightsModel.DateOneWay = Properties[0].DateOneWay.ToString("yyyy-MM-dd");
            _dataOfFlightsModel.DateReturn = Properties[0].DateReturn.ToString("yyyy-MM-dd");
            string param = _jsonConverter.Serialize(_dataOfFlightsModel);
            ShowViewModel<FlightsListViewModel>(new { param });
        }

        private void ClearFields()
        {
            Properties[0].IsCheckedOneWay = true;
            Properties[0].IsCheckedReturn = false;
            Properties[0].IsEnabledDateReturn = false;
            Properties[0].IsEnabledButtonFind = false;
            Properties[0].IsEnabledCityFrom = false;
            Properties[0].IsEnabledCityTo = false;
            Properties[0].IsVisibleChange = false;
            Properties[0].PlaceholderTextCityFrom = "Choose city";
            Properties[0].PlaceholderTextCityTo = "Choose city";
            Properties[0].TextCountryFrom = null;
            Properties[0].TextCountryTo = null;
            Properties[0].TextCityFrom = null;
            Properties[0].TextCityTo = null;
            RaisePropertyChanged(() => Properties);
        }

        private void ChangeFields()
        {
            Properties[0].TextCountryFrom = _dataOfFlightsModel.CountryTo;
            Properties[0].TextCountryTo = _dataOfFlightsModel.CountryFrom;
            Properties[0].TextCityFrom = _dataOfFlightsModel.CityTo;
            Properties[0].TextCityTo = _dataOfFlightsModel.CityFrom;
            List<string> cities = _dataOfFlightsModel.CitiesFrom;
            _dataOfFlightsModel.CitiesFrom = _dataOfFlightsModel.CitiesTo;
            _dataOfFlightsModel.CitiesTo = cities;
            RaisePropertyChanged(() => Properties);
            List<string> iata = _dataOfFlightsModel.IatasFrom;
            _dataOfFlightsModel.IatasFrom = _dataOfFlightsModel.IatasTo;
            _dataOfFlightsModel.IatasTo = iata;
            _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
            _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
            _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
            _dataOfFlightsModel.CityTo = Properties[0].TextCityTo;
            RaisePropertyChanged(() => Properties);
        }

        private void SetTheVisibilityIcon()
        {
            if (Properties[0].PivotNumber == 1 || Properties[0].PivotNumber == 2)
            {
                Properties[0].IsVisibleRefresh = (Properties[0].PivotNumber == 1) ? true : false;
                Properties[0].IsVisibleChange = false;
                Properties[0].IsVisibleClear = false;
            }
            else
            {
                Properties[0].IsVisibleRefresh = false;
                Properties[0].IsVisibleChange = _status;
                Properties[0].IsVisibleClear = true;
            }
            RaisePropertyChanged(() => Properties);
        }

        private void UpdateFavoriteList()
        {
            Properties[0].FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            if (Properties[0].FavoriteList == null)
            {
                Properties[0].FavoriteList = new ObservableCollection<FavoriteModel>();
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCountryFromAsync()
        {
            _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
            CitiesService citiesService = new CitiesService(_httpService, _jsonConverter);
            _dataOfFlightsModel.CitiesFrom = await citiesService.GetCities(_dataOfFlightsModel.CountryFrom);
            Properties[0].IsVisibleChange = (Properties[0].IsEnabledCityTo) ? true : false;
            _status = Properties[0].IsVisibleChange;
            if (_dataOfFlightsModel.CitiesFrom != null)
            {
                Properties[0].CitiesFrom = _dataOfFlightsModel.CitiesFrom;
                Properties[0].IsEnabledCityFrom = true;
                Properties[0].PlaceholderTextCityFrom = "Choose city";
            }
            else
            {
                Properties[0].TextCityFrom = null;
                Properties[0].PlaceholderTextCityFrom = "No available airports";
                Properties[0].CitiesFrom = null;
                Properties[0].IsEnabledCityFrom = false;
                Properties[0].IsEnabledButtonFind = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCountryToAsync()
        {
            _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
            CitiesService citiesService = new CitiesService(_httpService, _jsonConverter);
            _dataOfFlightsModel.CitiesTo = await citiesService.GetCities(_dataOfFlightsModel.CountryTo);
            Properties[0].IsVisibleChange = (Properties[0].IsEnabledCityFrom) ? true : false;
            _status = Properties[0].IsVisibleChange;
            if (_dataOfFlightsModel.CitiesTo != null)
            {
                Properties[0].CitiesTo = _dataOfFlightsModel.CitiesTo;
                Properties[0].IsEnabledCityTo = true;
                Properties[0].PlaceholderTextCityTo = "Choose city";
            }
            else
            {
                Properties[0].TextCityTo = null;
                Properties[0].PlaceholderTextCityTo = "No available airports";
                Properties[0].CitiesTo = null;
                Properties[0].IsEnabledCityTo = false;
                Properties[0].IsEnabledButtonFind = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityFromAsync()
        {
            _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
            IataService iataService = new IataService(_httpService, _jsonConverter);
            _dataOfFlightsModel.IatasFrom = await iataService.GetIata(_dataOfFlightsModel.CityFrom);
            if (Properties[0].TextCityFrom != null && Properties[0].TextCityTo != null && _dataOfFlightsModel.IatasFrom != null && _dataOfFlightsModel.IatasTo != null)
            {
                Properties[0].IsEnabledButtonFind = true;
            }
            else
            {
                Properties[0].IsEnabledButtonFind = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityToAsync()
        {
            _dataOfFlightsModel.CityTo = Properties[0].TextCityTo; IataService iataService = new IataService(_httpService, _jsonConverter);
            _dataOfFlightsModel.IatasTo = await iataService.GetIata(_dataOfFlightsModel.CityTo);
            if (Properties[0].TextCityFrom != null && Properties[0].TextCityTo != null && _dataOfFlightsModel.IatasTo != null && _dataOfFlightsModel.IatasFrom != null)
            {
                Properties[0].IsEnabledButtonFind = true;
            }
            else
            {
                Properties[0].IsEnabledButtonFind = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private void SetFlight(object arg)
        {
            if (arg is FavoriteModel)
            {
                FavoriteModel item = (FavoriteModel)arg;
                Properties[0].TextCountryFrom = item.CountryFrom;
                Properties[0].TextCountryTo = item.CountryTo;
                Properties[0].TextCityFrom = item.CityFrom;
                Properties[0].TextCityTo = item.CityTo;
                _dataOfFlightsModel.IatasFrom = item.IataFrom;
                _dataOfFlightsModel.IatasTo = item.IataTo;
                _dataOfFlightsModel.CitiesFrom = item.CitiesFrom;
                _dataOfFlightsModel.CitiesTo = item.CitiesTo;
                _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
                _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
                _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
                _dataOfFlightsModel.CityTo = Properties[0].TextCityTo;
                Properties[0].IsEnabledCityFrom = true;
                Properties[0].IsEnabledCityTo = true;
                Properties[0].IsEnabledButtonFind = true;
                _status = true;
                Properties[0].PivotNumber = 0;
                RaisePropertyChanged(() => Properties);
            }
        }        
    }
}

