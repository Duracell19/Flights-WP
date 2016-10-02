using Flights.Infrastructure;
using Flights.Models;
using Flights.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.File;
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
        private readonly IDateService _dateService;
        private readonly IJsonConverterService _jsonConverter;
        private readonly IMvxFileStore _fileStore;
        private ObservableCollection<MainPagePropetiesModel> _properties;
        private ObservableCollection<MainPageCommandsModel> _commands;
        private DataOfFlightsModel _dataOfFlightsModel;                    
        private bool _status;

        public ObservableCollection<MainPagePropetiesModel> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }
        public ObservableCollection<MainPageCommandsModel> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public MainPageViewModel(ICountriesService countriesService,
            ICitiesService citiesService,
            IHttpService httpService,
            IDateService dateService,
            IJsonConverterService jsonConverter,
            IMvxFileStore fileStore)
        {
            _countriesService = countriesService;
            _citiesService = citiesService;
            _httpService = httpService;
            _dateService = dateService;
            _jsonConverter = jsonConverter;
            _fileStore = fileStore;

            _dataOfFlightsModel = new DataOfFlightsModel();
            _commands = new ObservableCollection<MainPageCommandsModel>();
            _properties = new ObservableCollection<MainPagePropetiesModel>();
            
            Properties.Add(new MainPagePropetiesModel
            {
                PlaceholderTextCityFrom = "Choose city",
                PlaceholderTextCityTo = "Choose city",
                DateOneWay = DateTimeOffset.Now,
                DateReturn = DateTimeOffset.Now,
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
            Commands[0].SetTheVisibilityIconCommand = new MvxCommand(SetTheVisibilityIcon);
            Commands[0].SelectCountryFromCommand = new MvxCommand(SelectCountryFromAsync);
            Commands[0].SelectCountryToCommand = new MvxCommand(SelectCountryToAsync);
            Commands[0].SelectCityFromCommand = new MvxCommand(SelectCityFromAsync);
            Commands[0].SelectCityToCommand = new MvxCommand(SelectCityToAsync);
            Commands[0].SetFlightCommand = new MvxCommand<object>(SetFlight);
            Commands[0].DeleteFavoriteFlightCommand = new MvxCommand<object>(DeleteFavoriteFlight);
        }

        public override void Start()
        {
            base.Start();
        }

        public void Init(DataOfFlightsModel dataOfFlightsModel)
        {
            Properties[0].FavoriteList = Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            if (Properties[0].FavoriteList == null)
            {
                Properties[0].FavoriteList = new ObservableCollection<FavoriteModel>();
            }
            if (dataOfFlightsModel.CountryFrom != null)
            {
                _dataOfFlightsModel = dataOfFlightsModel;
                Properties[0].TextCountryFrom = _dataOfFlightsModel.CountryFrom;
                Properties[0].TextCountryTo = _dataOfFlightsModel.CountryTo;
                Properties[0].TextCityFrom = _dataOfFlightsModel.CityFrom;
                Properties[0].TextCityTo = _dataOfFlightsModel.CityTo;
                if (_dataOfFlightsModel.CitiesF != null)
                {
                    _dataOfFlightsModel.CitiesFrom = _jsonConverter.Deserialize<List<string>>(dataOfFlightsModel.CitiesF);
                    _dataOfFlightsModel.CitiesTo = _jsonConverter.Deserialize<List<string>>(dataOfFlightsModel.CitiesT);
                    _dataOfFlightsModel.IataFrom = _jsonConverter.Deserialize<List<string>>(dataOfFlightsModel.IataF);
                    _dataOfFlightsModel.IataTo = _jsonConverter.Deserialize<List<string>>(dataOfFlightsModel.IataT);
                    Properties[0].DateOneWay = DateTimeOffset.Parse(_dataOfFlightsModel.DateOneWayOffSet);
                    if (_dataOfFlightsModel.ReturnWay)
                    {
                        Properties[0].IsEnabledDateReturn = true;
                        Properties[0].DateReturn = DateTimeOffset.Parse(_dataOfFlightsModel.DateReturnOffSet);
                        Properties[0].IsCheckedReturn = true;
                        Properties[0].IsCheckedOneWay = false;
                    }
                    Properties[0].IsEnabledCityFrom = true;
                    Properties[0].IsEnabledCityTo = true;
                    Properties[0].IsEnabledButtonFind = true;
                    _status = true;
                }
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
            _dataOfFlightsModel.DateOneWay = _dateService.GetDate(Properties[0].DateOneWay);
            _dataOfFlightsModel.DateReturn = _dateService.GetDate(Properties[0].DateReturn);
            _dataOfFlightsModel.DateOneWayOffSet = _dateService.ConvertDate(Properties[0].DateOneWay);
            _dataOfFlightsModel.DateReturnOffSet = _dateService.ConvertDate(Properties[0].DateReturn);
            _dataOfFlightsModel.CitiesF = _jsonConverter.Serialize(_dataOfFlightsModel.CitiesFrom);
            _dataOfFlightsModel.CitiesT = _jsonConverter.Serialize(_dataOfFlightsModel.CitiesTo);
            _dataOfFlightsModel.IataF = _jsonConverter.Serialize(_dataOfFlightsModel.IataFrom);
            _dataOfFlightsModel.IataT = _jsonConverter.Serialize(_dataOfFlightsModel.IataTo);
            ShowViewModel<FlightsListViewModel>(_dataOfFlightsModel);
        }

        private void ClearFields()
        {
            _dataOfFlightsModel.CountryFrom = "";
            _dataOfFlightsModel.CountryTo = "";
            _dataOfFlightsModel.CityFrom = "";
            _dataOfFlightsModel.CityTo = "";
            Properties[0].IsCheckedOneWay = true;
            Properties[0].IsCheckedReturn = false;
            Properties[0].IsEnabledDateReturn = false;
            Properties[0].IsEnabledButtonFind = false;
            Properties[0].IsEnabledCityFrom = false;
            Properties[0].IsEnabledCityTo = false;
            Properties[0].IsEnabledChange = false;
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
            string value = Properties[0].TextCountryTo;
            Properties[0].TextCountryTo = Properties[0].TextCountryFrom;
            Properties[0].TextCountryFrom = value;
            value = Properties[0].TextCityTo;
            Properties[0].TextCityTo = Properties[0].TextCityFrom;
            Properties[0].TextCityFrom = value;
            _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
            _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
            _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
            _dataOfFlightsModel.CityTo = Properties[0].TextCityTo;
            List<string> iata = _dataOfFlightsModel.IataFrom; 
            _dataOfFlightsModel.IataFrom = _dataOfFlightsModel.IataTo;
            _dataOfFlightsModel.IataTo = iata;
            List<string> cities = _dataOfFlightsModel.CitiesFrom;
            _dataOfFlightsModel.CitiesFrom = _dataOfFlightsModel.CitiesTo;
            _dataOfFlightsModel.CitiesTo = cities;
            RaisePropertyChanged(() => Properties);
        }

        private void SetTheVisibilityIcon()
        {
            int number = Properties[0].PivotNumber;
            if (number == 1 || number == 2)
            {
                Properties[0].IsEnabledChange = false;
                Properties[0].IsEnabledClear = false;
            }
            else
            {
                Properties[0].IsEnabledChange = _status;
                Properties[0].IsEnabledClear = true;
            }
            RaisePropertyChanged(() => Properties);
        }

        private async void SelectCountryFromAsync()
        {
            _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
            CitiesService citiesService = new CitiesService(_httpService, _jsonConverter);
            _dataOfFlightsModel.CitiesFrom = await citiesService.GetCities(_dataOfFlightsModel.CountryFrom);
            Properties[0].IsEnabledChange = (Properties[0].IsEnabledCityTo) ? true : false;
            _status = Properties[0].IsEnabledChange;
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
            Properties[0].IsEnabledChange = (Properties[0].IsEnabledCityFrom) ? true : false;
            _status = Properties[0].IsEnabledChange;
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
            _dataOfFlightsModel.IataFrom = await iataService.GetIata(_dataOfFlightsModel.CityFrom);
            if (Properties[0].TextCityFrom != null && Properties[0].TextCityTo != null && _dataOfFlightsModel.IataFrom != null)
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
            _dataOfFlightsModel.IataTo = await iataService.GetIata(_dataOfFlightsModel.CityTo);
            if (Properties[0].TextCityFrom != null && Properties[0].TextCityTo != null && _dataOfFlightsModel.IataTo != null)
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
            if(arg is FavoriteModel)
            {
                FavoriteModel item = (FavoriteModel)arg;
                Properties[0].TextCountryFrom = item.CountryFrom;
                Properties[0].TextCountryTo = item.CountryTo;
                Properties[0].TextCityFrom = item.CityFrom;
                Properties[0].TextCityTo = item.CityTo;
                _dataOfFlightsModel.IataFrom = item.IataFrom;
                _dataOfFlightsModel.IataTo = item.IataTo;
                _dataOfFlightsModel.CitiesFrom = item.CitiesFrom;
                _dataOfFlightsModel.CitiesTo = item.CitiesTo;
                _dataOfFlightsModel.CountryFrom = Properties[0].TextCountryFrom;
                _dataOfFlightsModel.CountryTo = Properties[0].TextCountryTo;
                _dataOfFlightsModel.CityFrom = Properties[0].TextCityFrom;
                _dataOfFlightsModel.CityTo = Properties[0].TextCityTo;
                _dataOfFlightsModel.ReturnWay = false;
                Properties[0].IsEnabledCityFrom = true;
                Properties[0].IsEnabledCityTo = true;
                Properties[0].IsEnabledButtonFind = true;
                _status = true;
                Properties[0].PivotNumber = 0;
                RaisePropertyChanged(() => Properties);
            }
        }

        private void DeleteFavoriteFlight(object arg)
        {
            if (arg is FavoriteModel)
            {
                FavoriteModel item = (FavoriteModel)arg;
                Properties[0].FavoriteList.Remove(item);
                Save(Defines.FAVORITE_LIST_FILE_NAME, Properties[0].FavoriteList);
                Properties[0].PivotNumber = 1;
                RaisePropertyChanged(() => Properties);
            }
        }

        private T Load<T>(string fileName)
        {
            string txt;
            T result = default(T);
            if (_fileStore.TryReadTextFile(fileName, out txt))
            {
                return _jsonConverter.Deserialize<T>(txt);
            }
            return result;
        }
        
        private void Save(string fileName, object obj)
        {
            _fileStore.WriteFile(fileName, _jsonConverter.Serialize(obj));
        }
    }
}

