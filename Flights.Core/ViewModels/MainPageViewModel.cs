using Flights.Core.Commands;
using Flights.Infrastructure;
using Flights.Models;
using Flights.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.File;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Flights.Core.ViewModels
{
    public class MainPageViewModel : MvxViewModel
    {
        readonly ICountriesService _countriesService;
        readonly ICitiesService _citiesService;
        readonly IHttpService _httpService;
        readonly IDateService _dateService;
        readonly ISerializXMLService _serializService;
        readonly IDeserializXMLService _deserializService;
        readonly IJsonConverter _jsonConverter;
        private readonly IMvxFileStore _fileStore;

        MainPageModel mainPageModel = new MainPageModel();

        public MainPageViewModel(ICountriesService countriesService, ICitiesService citiesService, IHttpService httpService,
            IDateService dateService, ISerializXMLService serializService, IDeserializXMLService deserializService, 
            IJsonConverter jsonConverter,
            IMvxFileStore fileStore)
        {
            _countriesService = countriesService;
            _citiesService = citiesService;
            _httpService = httpService;
            _dateService = dateService;
            _serializService = serializService;
            _deserializService = deserializService;
            _jsonConverter = jsonConverter;
            _fileStore = fileStore;
        }

        public override void Start()
        {
            base.Start();
        }

        public void Init(MainPageModel _mainPageModel)
        {
            _fileStore.TryReadBinaryFile("favoriteList.xml", (inputStream) =>
            {
                return LoadFrom(inputStream);
            });

            if (_mainPageModel.CountryFrom != null)
            {
                mainPageModel = _mainPageModel;
                TextCountryFrom = mainPageModel.CountryFrom;
                TextCountryTo = mainPageModel.CountryTo;
                TextCityFrom = mainPageModel.CityFrom;
                TextCityTo = mainPageModel.CityTo;
                if (mainPageModel.CitiesF != null)
                {
                    mainPageModel.CitiesFrom = _jsonConverter.Deserialize<string[]>(_mainPageModel.CitiesF);
                    mainPageModel.CitiesTo = _jsonConverter.Deserialize<string[]>(_mainPageModel.CitiesT);
                    mainPageModel.IataFrom = _jsonConverter.Deserialize<string[]>(_mainPageModel.IataF);
                    mainPageModel.IataTo = _jsonConverter.Deserialize<string[]>(_mainPageModel.IataT);
                    DateOneWay = DateTimeOffset.Parse(mainPageModel.DateOneWayOffSet);
                    if (mainPageModel.ReturnWay)
                    {
                        IsEnabledDateReturn = true;
                        DateReturn = DateTimeOffset.Parse(mainPageModel.DateReturnOffSet);
                        IsCheckedReturn = true;
                        IsCheckedOneWay = false;
                    }
                    IsEnabledCityFrom = true;
                    IsEnabledCityTo = true;
                    IsEnabledButtonFind = true;
                    status = true;
                }
            }
        }

        bool isEnabledCityFrom = false;
        public bool IsEnabledCityFrom
        {
            get
            {
                return isEnabledCityFrom;
            }
            set
            {
                isEnabledCityFrom = value;
                RaisePropertyChanged(() => this.IsEnabledCityFrom);
            }
        }
        bool isEnabledCityTo = false;
        public bool IsEnabledCityTo
        {
            get
            {
                return isEnabledCityTo;
            }
            set
            {
                isEnabledCityTo = value;
                RaisePropertyChanged(() => this.IsEnabledCityTo);
            }
        }
        bool isCheckedOneWay = true;
        public bool IsCheckedOneWay
        {
            get
            {
                return isCheckedOneWay;
            }
            set
            {
                isCheckedOneWay = value;
                RaisePropertyChanged(() => this.IsCheckedOneWay);
            }
        }
        bool isCheckedReturn = false;
        public bool IsCheckedReturn
        {
            get
            {
                return isCheckedReturn;
            }
            set
            {
                isCheckedReturn = value;
                RaisePropertyChanged(() => this.IsCheckedReturn);
            }
        }
        bool isEnabledDateReturn = false;
        public bool IsEnabledDateReturn
        {
            get
            {
                return isEnabledDateReturn;
            }
            set
            {
                isEnabledDateReturn = value;
                RaisePropertyChanged(() => this.IsEnabledDateReturn);
            }
        }
        bool isEnabledButtonFind = false;
        public bool IsEnabledButtonFind
        {
            get
            {
                return isEnabledButtonFind;
            }
            set
            {
                isEnabledButtonFind = value;
                RaisePropertyChanged(() => this.IsEnabledButtonFind);
            }
        }
        bool isEnabledChange = false;
        public bool IsEnabledChange
        {
            get
            {
                return isEnabledChange;
            }
            set
            {
                isEnabledChange = value;
                RaisePropertyChanged(() => this.IsEnabledChange);
            }
        }
        bool isEnabledClear = true;
        public bool IsEnabledClear
        {
            get
            {
                return isEnabledClear;
            }
            set
            {
                isEnabledClear = value;
                RaisePropertyChanged(() => this.IsEnabledClear);
            }
        }
        string placeholderTextCityFrom = "Choose city";
        public string PlaceholderTextCityFrom
        {
            get
            {
                return placeholderTextCityFrom;
            }
            set
            {
                placeholderTextCityFrom = value;
                RaisePropertyChanged(() => this.PlaceholderTextCityFrom);
            }
        }
        string placeholderTextCityTo = "Choose city";
        public string PlaceholderTextCityTo
        {
            get
            {
                return placeholderTextCityTo;
            }
            set
            {
                placeholderTextCityTo = value;
                RaisePropertyChanged(() => this.PlaceholderTextCityTo);
            }
        }
        string textCountryFrom = "";
        public string TextCountryFrom
        {
            get
            {
                return textCountryFrom;
            }
            set
            {
                textCountryFrom = value;
                RaisePropertyChanged(() => this.TextCountryFrom);
            }
        }
        string textCountryTo = "";
        public string TextCountryTo
        {
            get
            {
                return textCountryTo;
            }
            set
            {
                textCountryTo = value;
                RaisePropertyChanged(() => this.TextCountryTo);
            }
        }
        string textCityFrom = "";
        public string TextCityFrom
        {
            get
            {
                return textCityFrom;
            }
            set
            {
                textCityFrom = value;
                RaisePropertyChanged(() => this.TextCityFrom);
            }
        }
        string textCityTo = "";
        public string TextCityTo
        {
            get
            {
                return textCityTo;
            }
            set
            {
                textCityTo = value;
                RaisePropertyChanged(() => this.TextCityTo);
            }
        }
        DateTimeOffset dateOneWay = DateTimeOffset.Now;
        public DateTimeOffset DateOneWay
        {
            get
            {
                return dateOneWay;
            }
            set
            {
                dateOneWay = value;
                RaisePropertyChanged(() => this.DateOneWay);
            }
        }
        DateTimeOffset dateReturn = DateTimeOffset.Now;
        public DateTimeOffset DateReturn
        {
            get
            {
                return dateReturn;
            }
            set
            {
                dateReturn = value;
                RaisePropertyChanged(() => this.DateReturn);
            }
        }
        int pivotNumber;
        public int PivotNumber
        {
            get
            {
                return pivotNumber;
            }
            set
            {
                pivotNumber = value;
                RaisePropertyChanged(() => this.PivotNumber);
            }
        }
        bool status;
        
        ObservableCollection<string> countriesFrom = new ObservableCollection<string>();
        public ObservableCollection<string> CountriesFrom
        {
            get
            {
                if (countriesFrom.Count == 0)
                {
                    string[] country = _countriesService.GetCountries();
                    foreach (string s in country)
                    {
                        countriesFrom.Add(s);
                    }
                }
                return countriesFrom;
            }
            set
            {
                countriesFrom = value;
                RaisePropertyChanged(() => this.CountriesFrom);
            }
        }
        ObservableCollection<string> countriesTo = new ObservableCollection<string>();
        public ObservableCollection<string> CountriesTo
        {
            get
            {
                if (countriesTo.Count == 0)
                {
                    string[] country = _countriesService.GetCountries();
                    foreach (string s in country)
                    {
                        countriesTo.Add(s);
                    }
                }
                return countriesTo;
            }
            set
            {
                countriesTo = value;
                RaisePropertyChanged(() => this.CountriesTo);
            }
        }
        ObservableCollection<string> citiesFrom = new ObservableCollection<string>();
        public ObservableCollection<string> CitiesFrom
        {
            get
            {
                return citiesFrom;
            }
            set
            {
                citiesFrom = value;
                RaisePropertyChanged(() => this.CitiesFrom);
            }
        }
        ObservableCollection<string> citiesTo = new ObservableCollection<string>();
        public ObservableCollection<string> CitiesTo
        {
            get
            {
                return citiesTo;
            }
            set
            {
                citiesTo = value;
                RaisePropertyChanged(() => this.CitiesTo);
            }
        }
        ObservableCollection<FavoriteModel> favoriteList = new ObservableCollection<FavoriteModel>();
        public ObservableCollection<FavoriteModel> FavoriteList
        {
            get
            {
                return favoriteList;
            }
            set
            {
                favoriteList = value;
                RaisePropertyChanged(() => this.FavoriteList);
            }
        }

        ICommand textChangedCountryFrom;
        public ICommand TextChangedCountryFrom
        {
            get
            {
                return textChangedCountryFrom
                    ?? (textChangedCountryFrom = new ActionCommand(() =>
                    {
                        if (TextCountryFrom.Length > -1)
                        {
                            string[] s = _countriesService.GetCountries();
                            ObservableCollection<string> result = new ObservableCollection<string>();
                            foreach (string st in s)
                            {
                                if (st.Contains(TextCountryFrom))
                                {
                                    result.Add(st);
                                }
                            }
                            CountriesFrom = result;
                        }
                    }));
            }
        }
        ICommand textChangedCountryTo;
        public ICommand TextChangedCountryTo
        {
            get
            {
                return textChangedCountryTo
                    ?? (textChangedCountryTo = new ActionCommand(() =>
                    {
                        if (TextCountryTo.Length > -1)
                        {
                            string[] s = _countriesService.GetCountries();
                            ObservableCollection<string> result = new ObservableCollection<string>();
                            foreach (string st in s)
                            {
                                if (st.Contains(TextCountryTo))
                                {
                                    result.Add(st);
                                }
                            }
                            CountriesTo = result;
                        }
                    }));
            }
        }
        ICommand textChangedCityFrom;
        public ICommand TextChangedCityFrom
        {
            get
            {
                return textChangedCityFrom
                    ?? (textChangedCityFrom = new ActionCommand(() =>
                    {
                        if (TextCityFrom.Length > -1)
                        {
                            if (mainPageModel.CitiesFrom != null)
                            {
                                string[] s = mainPageModel.CitiesFrom;
                                ObservableCollection<string> result = new ObservableCollection<string>();
                                foreach (string st in s)
                                {
                                    if (st.Contains(TextCityFrom))
                                    {
                                        result.Add(st);
                                    }
                                }
                                CitiesFrom = result;
                            }
                        }
                        else
                            IsEnabledButtonFind = false;
                    }));
            }
        }
        ICommand textChangedCityTo;
        public ICommand TextChangedCityTo
        {
            get
            {
                return textChangedCityTo
                    ?? (textChangedCityTo = new ActionCommand(() =>
                    {
                        if (TextCityTo.Length > -1)
                        {
                            if (mainPageModel.CitiesTo != null)
                            {
                                string[] s = mainPageModel.CitiesTo;
                                ObservableCollection<string> result = new ObservableCollection<string>();
                                foreach (string st in s)
                                {
                                    if (st.Contains(TextCityTo))
                                    {
                                        result.Add(st);
                                    }
                                }
                                CitiesTo = result;
                            }
                        }
                        else
                            IsEnabledButtonFind = false;
                    }));
            }
        }
        MvxCommand<string> chosenItemCountryFrom;
        public ICommand ChosenItemCountryFrom
        {
            get { return chosenItemCountryFrom ?? (chosenItemCountryFrom = new MvxCommand<string>(arg => this.ChooseCountryFrom(arg))); }
        }
        public async void ChooseCountryFrom(string arg)
        {
            mainPageModel.CountryFrom = arg;
            TextCityFrom = "";
            IsEnabledCityFrom = false;
            IsEnabledButtonFind = false;
            CitiesService citiesService = new CitiesService(_httpService, _jsonConverter);
            mainPageModel.CitiesFrom = await citiesService.GetCities(mainPageModel.CountryFrom);
            if (IsEnabledCityTo == true)
            {
                IsEnabledChange = true;
            }
            else
            {
                IsEnabledChange = false;
            }
            status = IsEnabledChange;
            if (mainPageModel.CitiesFrom != null && mainPageModel.CitiesFrom.Length != 0)
            {
                ObservableCollection<string> result = new ObservableCollection<string>();
                foreach (string s in mainPageModel.CitiesFrom)
                {
                    result.Add(s);
                }
                CitiesFrom = result;
                IsEnabledCityFrom = true;
                PlaceholderTextCityFrom = "Choose city";
            }
            else
            {
                CitiesFrom = null;
                PlaceholderTextCityFrom = "No available airports";
                IsEnabledCityFrom = false;
                IsEnabledButtonFind = false;
            }
        }
        MvxCommand<string> chosenItemCountryTo;
        public ICommand ChosenItemCountryTo
        {
            get { return chosenItemCountryTo ?? (chosenItemCountryTo = new MvxCommand<string>(arg => this.ChooseCountryTo(arg))); }
        }
        public async void ChooseCountryTo(string arg)
        {
            mainPageModel.CountryTo = arg;
            TextCityTo = "";
            IsEnabledCityTo = false;
            IsEnabledButtonFind = false;
            CitiesService citiesService = new CitiesService(_httpService, _jsonConverter);
            mainPageModel.CitiesTo = await citiesService.GetCities(mainPageModel.CountryTo);
            if (IsEnabledCityFrom == true)
            {
                IsEnabledChange = true;
            }
            else
            {
                IsEnabledChange = false;
            }
            status = IsEnabledChange;
            if (mainPageModel.CitiesTo != null && mainPageModel.CitiesTo.Length != 0)
            {
                ObservableCollection<string> result = new ObservableCollection<string>();
                foreach (string s in mainPageModel.CitiesTo)
                {
                    result.Add(s);
                }
                CitiesTo = result;
                IsEnabledCityTo = true;
                PlaceholderTextCityTo = "Choose city";
            }
            else
            {
                CitiesTo = null;
                PlaceholderTextCityTo = "No available airports";
                IsEnabledCityTo = false;
                IsEnabledButtonFind = false;
            }
        }
        MvxCommand<string> chosenItemCityFrom;
        public ICommand ChosenItemCityFrom
        {
            get { return chosenItemCityFrom ?? (chosenItemCityFrom = new MvxCommand<string>(arg => this.ChooseCityFrom(arg))); }
        }
        public async void ChooseCityFrom(string arg)
        {
            mainPageModel.CityFrom = arg;
            IataService iataService = new IataService(_httpService, _jsonConverter);
            mainPageModel.IataFrom = await iataService.GetIata(mainPageModel.CityFrom);
            if (TextCityFrom.Length != 0 && TextCityTo.Length != 0 && mainPageModel.IataFrom != null)
                IsEnabledButtonFind = true;
            else
                IsEnabledButtonFind = false;
        }
        MvxCommand<string> chosenItemCityTo;
        public ICommand ChosenItemCityTo
        {
            get { return chosenItemCityTo ?? (chosenItemCityTo = new MvxCommand<string>(arg => this.ChooseCityTo(arg))); }
        }
        public async void ChooseCityTo(string arg)
        {
            mainPageModel.CityTo = arg;
            IataService iataService = new IataService(_httpService, _jsonConverter);
            mainPageModel.IataTo = await iataService.GetIata(mainPageModel.CityTo);
            if (TextCityFrom.Length != 0 && TextCityTo.Length != 0 && mainPageModel.IataTo != null)
                IsEnabledButtonFind = true;
            else
                IsEnabledButtonFind = false;
        }
        ICommand oneWayCommand;
        public ICommand OneWayCommand
        {
            get
            {
                return oneWayCommand
                    ?? (oneWayCommand = new ActionCommand(() =>
                    {
                        IsCheckedOneWay = true;
                        IsCheckedReturn = false;
                        mainPageModel.ReturnWay = false;
                        IsEnabledDateReturn = false;
                    }));
            }
        }
        ICommand returnCommand;
        public ICommand ReturnCommand
        {
            get
            {
                return returnCommand
                    ?? (returnCommand = new ActionCommand(() =>
                    {
                        IsCheckedOneWay = false;
                        IsCheckedReturn = true;
                        mainPageModel.ReturnWay = true;
                        IsEnabledDateReturn = true;
                    }));
            }
        }
        ICommand findCommand;
        public ICommand FindCommand
        {
            get
            {
                return findCommand
                    ?? (findCommand = new ActionCommand(() =>
                    {
                        mainPageModel.DateOneWay = _dateService.GetDate(DateOneWay);
                        mainPageModel.DateReturn = _dateService.GetDate(DateReturn);
                        mainPageModel.DateOneWayOffSet = _dateService.ConvertDate(DateOneWay);
                        mainPageModel.DateReturnOffSet = _dateService.ConvertDate(DateReturn);
                        mainPageModel.CitiesF = _jsonConverter.Serialize(mainPageModel.CitiesFrom);
                        mainPageModel.CitiesT = _jsonConverter.Serialize(mainPageModel.CitiesTo);
                        mainPageModel.IataF = _jsonConverter.Serialize(mainPageModel.IataFrom);
                        mainPageModel.IataT = _jsonConverter.Serialize(mainPageModel.IataTo);
                        ShowViewModel<FlightsListViewModel>(mainPageModel);
                    }));
            }
        }
        ICommand clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return clearCommand
                    ?? (clearCommand = new ActionCommand(() =>
                    {
                        mainPageModel.CountryFrom = "";
                        mainPageModel.CountryTo = "";
                        mainPageModel.CityFrom = "";
                        mainPageModel.CityTo = "";
                        TextCountryFrom = "";
                        TextCountryTo = "";
                        TextCityFrom = "";
                        TextCityTo = "";
                        PlaceholderTextCityFrom = "Choose city";
                        PlaceholderTextCityTo = "Choose city";
                        IsCheckedOneWay = true;
                        IsCheckedReturn = false;
                        IsEnabledDateReturn = false;
                        IsEnabledButtonFind = false;
                        IsEnabledCityFrom = false;
                        IsEnabledCityTo = false;
                        IsEnabledChange = false;
                    }));
            }
        }
        ICommand changeCommand;
        public ICommand ChangeCommand
        {
            get
            {
                return changeCommand
                    ?? (changeCommand = new ActionCommand(() =>
                    {
                        string value = TextCountryTo;
                        TextCountryTo = TextCountryFrom;
                        TextCountryFrom = value;
                        value = TextCityTo;
                        TextCityTo = TextCityFrom;
                        TextCityFrom = value;
                        mainPageModel.CountryFrom = TextCountryFrom;
                        mainPageModel.CountryTo = TextCountryTo;
                        mainPageModel.CityFrom = TextCityFrom;
                        mainPageModel.CityTo = TextCityTo;
                        string[] iata = mainPageModel.IataFrom;
                        mainPageModel.IataFrom = mainPageModel.IataTo;
                        mainPageModel.IataTo = iata;
                        string[] cities = mainPageModel.CitiesFrom;
                        mainPageModel.CitiesFrom = mainPageModel.CitiesTo;
                        mainPageModel.CitiesTo = cities;
                    }));
            }
        }
        ICommand helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                return helpCommand
                    ?? (helpCommand = new ActionCommand(() =>
                    {
                        ShowViewModel<HelpViewModel>();
                    }));
            }
        }
        ICommand pivotSelectionChanged;
        public ICommand PivotSelectionChanged
        {
            get
            {
                return pivotSelectionChanged
                    ?? (pivotSelectionChanged = new ActionCommand(() =>
                    {
                        int number = PivotNumber;
                        if (number == 1 || number == 2)
                        {
                            IsEnabledChange = false;
                            IsEnabledClear = false;
                        }
                        else
                        {
                            IsEnabledChange = status;
                            IsEnabledClear = true;
                        }
                    }));
            }
        }
        RelayCommand favoritesItemClick;
        public ICommand FavoritesItemClick
        {
            get
            {
                if (favoritesItemClick == null)
                {
                    favoritesItemClick = new RelayCommand(param => this.ItemClick(param));
                }
                return favoritesItemClick;
            }
        }
        public void ItemClick(object arg)
        {
            FavoriteModel item = (FavoriteModel)arg;
            TextCountryFrom = item.CountryFrom;
            TextCountryTo = item.CountryTo;
            TextCityFrom = item.CityFrom;
            TextCityTo = item.CityTo;
            mainPageModel.IataFrom = item.IataFrom;
            mainPageModel.IataTo = item.IataTo;
            mainPageModel.CitiesFrom = item.CitiesFrom;
            mainPageModel.CitiesTo = item.CitiesTo;
            mainPageModel.CountryFrom = TextCountryFrom;
            mainPageModel.CountryTo = TextCountryTo;
            mainPageModel.CityFrom = TextCityFrom;
            mainPageModel.CityTo = TextCityTo;
            mainPageModel.ReturnWay = false;
            IsEnabledCityFrom = true;
            IsEnabledCityTo = true;
            IsEnabledButtonFind = true;
            status = true;
            PivotNumber = 0;
        }

        bool LoadFrom(Stream inputStream)
        {
            try
            {
                var loadedData = XDocument.Load(inputStream);
                if (loadedData.Root == null)
                    return false;

                using (var reader = loadedData.Root.CreateReader())
                {
                    var list = (ObservableCollection<FavoriteModel>)new XmlSerializer(typeof(ObservableCollection<FavoriteModel>)).Deserialize(reader);
                    favoriteList = new ObservableCollection<FavoriteModel>(list);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

