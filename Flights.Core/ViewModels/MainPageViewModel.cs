using Flights.Infrastructure;
using Flights.Infrastructure.Interfaces;
using Flights.Models;
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
        private readonly IIataService _iataService;
        private readonly IHttpService _httpService;
        private readonly IJsonConverter _jsonConverter;
        private readonly IFileStore _fileStore;
        private List<MainPagePropetiesModel> _properties;
        private List<MainPageCommandsModel> _commands;
        private DataOfFlightsModel _dataOfFlightsModel;

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
            _commands = new List<MainPageCommandsModel>();
            _properties = new List<MainPagePropetiesModel>();
            
            Commands.Add(new MainPageCommandsModel { });

            Commands[0].ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            Commands[0].SetOneWayCommand = new MvxCommand(SetOneWay);
            Commands[0].SetReturnWayCommand = new MvxCommand(SetReturnWay);
            Commands[0].FindFlightsCommand = new MvxCommand(FindFlights);
            Commands[0].ClearFieldsCommand = new MvxCommand(ClearFields);
            Commands[0].UpdateFavoriteListCommand = new MvxCommand(UpdateFavoriteList);
            Commands[0].SetTheVisibilityIconCommand = new MvxCommand(SetTheVisibilityIcon);
            Commands[0].SelectCountryFromCommand = new MvxCommand(SelectCountryFromAsync);
            Commands[0].SelectCountryToCommand = new MvxCommand(SelectCountryToAsync);
            Commands[0].SelectCityFromCommand = new MvxCommand(SelectCityFromAsync);
            Commands[0].SelectCityToCommand = new MvxCommand(SelectCityToAsync);
            Commands[0].SetFlightCommand = new MvxCommand<object>(SetFlight);
        }

        public void Init()
        {
            Properties.Add(SetMainPageProperties());
            Properties[0].FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
        }

        public override void Start()
        {
            base.Start();
        }
        
        private void SetOneWay()
        {
            Properties[0].IsCheckedOneWay = true;
            Properties[0].IsCheckedReturn = false;
            Properties[0].IsEnabledDateReturn = false;
            _dataOfFlightsModel.ReturnWay = false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetReturnWay()
        {
            Properties[0].IsCheckedOneWay = false;
            Properties[0].IsCheckedReturn = true;
            Properties[0].IsEnabledDateReturn = true;
            _dataOfFlightsModel.ReturnWay = true;
            RaisePropertyChanged(() => Properties);
        }

        private void FindFlights()
        {
            _dataOfFlightsModel.DateOneWay = Properties[0].DateOneWay.ToString("yyyy-MM-dd");
            _dataOfFlightsModel.DateReturn = Properties[0].DateReturn.ToString("yyyy-MM-dd");
            var param = _jsonConverter.Serialize(_dataOfFlightsModel);
            ShowViewModel<FlightsListViewModel>(new { param });
        }

        private void ClearFields()
        {
            Properties[0].TextCountryFrom = null;
            Properties[0].TextCountryTo = null;
            Properties[0].TextCityFrom = null;
            Properties[0].TextCityTo = null;
            Properties[0].IsCheckedOneWay = true;
            Properties[0].IsCheckedReturn = false;
            Properties[0].IsEnabledDateReturn = false;
            RaisePropertyChanged(() => Properties);
        }
        
        private void SetTheVisibilityIcon()
        {
            if (Properties[0].PivotNumber == 0)
            {
                Properties[0].VisibilityRefresh = false;
                Properties[0].VisibilityClear = true;
            }
            else
            {
                Properties[0].VisibilityRefresh = (Properties[0].PivotNumber == 1) ? true : false;
                Properties[0].VisibilityClear = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private void UpdateFavoriteList()
        {
            Properties[0].FavoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            RaisePropertyChanged(() => Properties);
        }

        public async void SelectCountryFromAsync()
        {
            Properties[0].CitiesFrom = await _citiesService.GetCities(Properties[0].TextCountryFrom);
            if (Properties[0].CitiesFrom != null)
            {
                Properties[0].IsEnabledCityFrom = true;
                Properties[0].PlaceholderTextCityFrom = "Choose city";
                _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
                _dataOfFlightsModel.CitiesFrom = Properties[0].CitiesFrom;
            }
            else
            {
                Properties[0].PlaceholderTextCityFrom = "No available airports";
                Properties[0].IsEnabledCityFrom = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCountryToAsync()
        {
            Properties[0].CitiesTo = await _citiesService.GetCities(Properties[0].TextCountryTo);
            if (Properties[0].CitiesTo != null)
            {
                Properties[0].IsEnabledCityTo = true;
                Properties[0].PlaceholderTextCityTo = "Choose city";
                _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
                _dataOfFlightsModel.CitiesTo = Properties[0].CitiesTo;
            }
            else
            {
                Properties[0].PlaceholderTextCityTo = "No available airports";
                Properties[0].IsEnabledCityTo = false;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityFromAsync()
        {
            _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
            _dataOfFlightsModel.IatasFrom = await _iataService.GetIata(Properties[0].TextCityFrom);
            Properties[0].IsEnabledButtonFind = (IsDataAboutFlightExist()) ? true : false;
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCityToAsync()
        {
            _dataOfFlightsModel.CityTo = Properties[0].TextCityTo;
            _dataOfFlightsModel.IatasTo = await _iataService.GetIata(Properties[0].TextCityTo);
            Properties[0].IsEnabledButtonFind = (IsDataAboutFlightExist()) ? true : false;
            RaisePropertyChanged(() => Properties);
        }

        private void SetFlight(object arg)
        {
            if (arg is FavoriteModel)
            {
                ClearFields();
                var item = (FavoriteModel)arg;
                Properties[0].TextCountryFrom = item.CountryFrom;
                Properties[0].TextCountryTo = item.CountryTo;
                Properties[0].TextCityFrom = item.CityFrom;
                Properties[0].TextCityTo = item.CityTo;
                Properties[0].PivotNumber = 0;
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
            return Properties[0].TextCityFrom != null && Properties[0].TextCityTo != null &&
                _dataOfFlightsModel.IatasTo != null && _dataOfFlightsModel.IatasFrom != null;
        }
    }
}

