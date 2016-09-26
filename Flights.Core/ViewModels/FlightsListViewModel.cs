using Flights.Core.Commands;
using Flights.Infrastructure;
using Flights.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.File;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Flights.Core.ViewModels
{
    public class FlightsListViewModel : MvxViewModel
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly IMvxFileStore _fileStore;
        private readonly IFlightsService _flightsService;

        private int count = 0;
        private MainPageModel _mainPageModel;
        private ObservableCollection<FavoriteModel> _addFavorite;
        private ObservableCollection<FavoriteModel> _favoriteList;
        private ObservableCollection<FlyInfoShowModel> _flightsList;
        private bool _isLoadProcess = true;
        private bool _isFlightAddedToFavorite;

        public ICommand ShowFlightDetailsCommand { get; set; }
        public ICommand ShowHelpInformationCommand { get; set; }
        public ICommand ShowAboutInforamtionCommand { get; set; }
        public ICommand AddToFavoritesCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public bool IsLoadProcess
        {
            get { return _isLoadProcess; }
            set
            {
                _isLoadProcess = value;
                RaisePropertyChanged(() => IsLoadProcess);
            }
        }

        public bool IsFlightAddedToFavorite
        {
            get { return _isFlightAddedToFavorite; }
            set
            {
                _isFlightAddedToFavorite = value;
                RaisePropertyChanged(() => IsFlightAddedToFavorite);
            }
        }

        public ObservableCollection<FlyInfoShowModel> FlightsList
        {
            get { return _flightsList; }
            set
            {
                _flightsList = value;
                RaisePropertyChanged(() => FlightsList);
            }
        }

        public FlightsListViewModel(
            IJsonConverter jsonConverter, 
            IMvxFileStore fileStore,
            IFlightsService flightsService)
        {
            _jsonConverter = jsonConverter;
            _flightsService = flightsService;
            _fileStore = fileStore;
            _mainPageModel = new MainPageModel();
            _addFavorite = new ObservableCollection<FavoriteModel>();
            _favoriteList = new ObservableCollection<FavoriteModel>();
            _flightsList = new ObservableCollection<FlyInfoShowModel>();
            
            BackCommand = new MvxCommand(() => ShowViewModel<MainPageViewModel>(_mainPageModel));
            ShowFlightDetailsCommand = new MvxCommand<FlyInfoShowModel>(ShowFlyDetails);
            ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            ShowAboutInforamtionCommand = new MvxCommand(() => ShowViewModel<AboutViewModel>());
            //AddToFavoritesCommand = new MvxCommand(AddToFavorites);
        }

        public void Init(MainPageModel mainPageModel)
        {
            _mainPageModel = mainPageModel; 
            _mainPageModel.CitiesFrom = _jsonConverter.Deserialize<string[]>(_mainPageModel.CitiesF);
            _mainPageModel.CitiesTo = _jsonConverter.Deserialize<string[]>(_mainPageModel.CitiesT);
            _mainPageModel.IataFrom = _jsonConverter.Deserialize<string[]>(_mainPageModel.IataF);
            _mainPageModel.IataTo = _jsonConverter.Deserialize<string[]>(_mainPageModel.IataT);
            _fileStore.TryReadBinaryFile("favoriteList.xml", (inputStream) =>
            {
                return LoadFrom(inputStream);
            });
            ShowFlightsAsync();
        }
        
        //private void AddToFavorites()
        //{
        //    AddFavorite();

        //    Save(Defines.FAVORITE_LIST_FILE_NAME, _favoriteList);

        //    IsFlightAddedToFavorite = false;
        //}

        //************ =>
        private ICommand favoriteCommand;
        public ICommand FavoriteCommand
        {
            get
            {
                return favoriteCommand
                    ?? (favoriteCommand = new ActionCommand(() =>
                    {
                        AddFavorite();
                        _fileStore.WriteFile("favoriteList.xml", (stream) =>
                                                                 {
                                                                     var serializer = new XmlSerializer(typeof(ObservableCollection<FavoriteModel>));
                                                                     serializer.Serialize(stream, _addFavorite);
                                                                 });
                        IsFlightAddedToFavorite = false;
                    }));
            }
        }
        //********** <=
        private void ShowFlyDetails(FlyInfoShowModel c)
        {
            ShowViewModel<FlightsInfoViewModel>(FlightsList.ElementAt(c.id));
        }

        async void ShowFlightsAsync()
        {
            FlyInfoModel[] _flyInfoOneWayModel = await _flightsService.ConfigurationOfFlights(_mainPageModel, _mainPageModel.DateOneWay, false);
            FlyInfoModel[] _flyInfoReturnModel = await _flightsService.ConfigurationOfFlights(_mainPageModel, _mainPageModel.DateReturn, true);
            await GenerateFlightsListAsync(_flyInfoOneWayModel, false);
            if (_mainPageModel.ReturnWay == true)
            {
                await GenerateFlightsListAsync(_flyInfoReturnModel, true);
            }
            IsLoadProcess = false;
            IsFlightAddedToFavorite = !(_favoriteList.Any(IsFlightEqualOfFavoriteModel));
        }

        async Task GenerateFlightsListAsync(FlyInfoModel[] flyInfoModel, bool returnWay)
        {
            string picture = "ms-appx:///Assets/fly.png";
            if (returnWay == true)
            {
                picture = "ms-appx:///Assets/fly_return.png";
            }
            foreach (var item in flyInfoModel)
            {
                if (item == null)
                {
                    continue;
                }
                for (int j = 0; j < item.Arrival.Length; j++)
                {
                    FlightsList.Add(new FlyInfoShowModel
                    {
                        Arrival = item.Arrival.ElementAt(j),
                        Duration = item.Duration.ElementAt(j),
                        ArrivalTerminal = item.ArrivalTerminal.ElementAt(j),
                        From = item.From.ElementAt(j),
                        ThreadCarrierTitle = item.ThreadCarrierTitle.ElementAt(j),
                        ThreadVehicle = item.ThreadVehicle.ElementAt(j),
                        ThreadNumber = item.ThreadNumber.ElementAt(j),
                        Departure = item.Departure.ElementAt(j),
                        To = item.To.ElementAt(j),
                        id = count,
                        Image1 = picture,
                        Image2 = "ms-appx:///Assets/direction.png"
                    });
                    count++;
                }
            }
        }

        void AddFavorite()
        {
            _addFavorite = _favoriteList;
            _addFavorite.Add(new FavoriteModel
            {
                CitiesFrom = _mainPageModel.CitiesFrom,
                CitiesTo = _mainPageModel.CitiesTo,
                CityFrom = _mainPageModel.CityFrom,
                CityTo = _mainPageModel.CityTo,
                CountryFrom = _mainPageModel.CountryFrom,
                CountryTo = _mainPageModel.CountryTo,
                IataFrom = _mainPageModel.IataFrom,
                IataTo = _mainPageModel.IataTo,
                Image1 = "ms-appx:///Assets/favorite.png",
                Image2 = "ms-appx:///Assets/direction.png"
            });
        }

        private bool IsFlightEqualOfFavoriteModel(FavoriteModel model)
        {
            return model.CountryFrom == _mainPageModel.CountryFrom && model.CityFrom == _mainPageModel.CityFrom
                   && model.CountryTo == _mainPageModel.CountryTo && model.CityTo == _mainPageModel.CityTo;
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
                    _favoriteList = new ObservableCollection<FavoriteModel>(list);
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