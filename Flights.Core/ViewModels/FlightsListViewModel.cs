using Flights.Infrastructure;
using Flights.Infrastructure.Interfaces;
using Flights.Models;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class FlightsListViewModel : MvxViewModel
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly IFileStore _fileStore;
        private readonly IFlightsService _flightsService;
        private DataOfFlightsModel _dataOfFlightsModel;
        private ObservableCollection<FavoriteModel> _favoriteList;
        private ObservableCollection<FlyInfoShowModel> _flightsList;
        private bool _isLoading;
        private bool _isFlightAlreadyInFavorite;

        public ICommand ShowFlightDetailsCommand { get; set; }
        public ICommand ShowHelpInformationCommand { get; set; }
        public ICommand ShowAboutInforamtionCommand { get; set; }
        public ICommand AddToFavoritesCommand { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public bool IsFlightAlreadyInFavorite
        {
            get { return _isFlightAlreadyInFavorite; }
            set
            {
                _isFlightAlreadyInFavorite = value;
                RaisePropertyChanged(() => IsFlightAlreadyInFavorite);
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
            IFileStore fileStore,
            IFlightsService flightsService)
        {
            _jsonConverter = jsonConverter;
            _flightsService = flightsService;
            _fileStore = fileStore;
            _flightsList = new ObservableCollection<FlyInfoShowModel>();

            ShowFlightDetailsCommand = new MvxCommand<object>(ShowFlyDetails);
            ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            ShowAboutInforamtionCommand = new MvxCommand(() => ShowViewModel<AboutViewModel>());
            AddToFavoritesCommand = new MvxCommand(AddToFavorites);
        }

        public void Init(string param)
        {
            IsFlightAlreadyInFavorite = true;
            _dataOfFlightsModel = _jsonConverter.Deserialize<DataOfFlightsModel>(param);
            _favoriteList = _fileStore.Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            ShowFlightsAsync();
        }

        private void AddToFavorites()
        {
            AddFavorite();
            _fileStore.Save(Defines.FAVORITE_LIST_FILE_NAME, _favoriteList);
            IsFlightAlreadyInFavorite = true;
        }

        private void ShowFlyDetails(object arg)
        {
            if (arg is FlyInfoShowModel)
            {
                FlyInfoShowModel item = (FlyInfoShowModel)arg;
                ShowViewModel<FlightsInfoViewModel>(arg);
            }
        }
        
        private void AddFavorite()
        {
            _favoriteList.Add(new FavoriteModel
            {
                CitiesFrom = _dataOfFlightsModel.CitiesFrom,
                CitiesTo = _dataOfFlightsModel.CitiesTo,
                CityFrom = _dataOfFlightsModel.CityFrom,
                CityTo = _dataOfFlightsModel.CityTo,
                CountryFrom = _dataOfFlightsModel.CountryFrom,
                CountryTo = _dataOfFlightsModel.CountryTo,
                IataFrom = _dataOfFlightsModel.IatasFrom,
                IataTo = _dataOfFlightsModel.IatasTo,
                Image1 = "ms-appx:///Assets/favorite.png"
            });
        }

        private async void ShowFlightsAsync()
        {
            IsLoading = true;

            await InitializeDataAsync(
                _dataOfFlightsModel.DateOneWay,
                _dataOfFlightsModel.IatasFrom,
                _dataOfFlightsModel.IatasTo);

            if (_dataOfFlightsModel.ReturnWay == true)
            {
                await InitializeDataAsync(
                            _dataOfFlightsModel.DateReturn,
                            _dataOfFlightsModel.IatasTo,
                            _dataOfFlightsModel.IatasFrom,
                            true);
            }

            if (_favoriteList != null)
            {
                IsFlightAlreadyInFavorite = _favoriteList.Any(IsFlightEqualOfFavoriteModel);
            }

            IsLoading = false;
        }

        private async Task InitializeDataAsync(string date, List<string> from, List<string> to, bool isReversed = false)
        {
            var flyInfoOneWayModel = await _flightsService.ConfigurationOfFlights(date, from, to);

            AddToFlightsList(flyInfoOneWayModel, isReversed);
        }

        private void AddToFlightsList(List<FlyInfoModel> flyInfoModel, bool isReversedFlight = false)
        {
            foreach (var item in flyInfoModel)
            {
                FlightsList.Add(CreateFlyInfoShowModel(item, isReversedFlight));
            }
        }

        private FlyInfoShowModel CreateFlyInfoShowModel(FlyInfoModel infoModel, bool isReversedFlight)
        {
            string picture = (isReversedFlight) ? "ms-appx:///Assets/fly_return.png" : "ms-appx:///Assets/fly.png";

            return new FlyInfoShowModel
            {
                Arrival = infoModel.Arrival,
                Duration = infoModel.Duration,
                ArrivalTerminal = infoModel.ArrivalTerminal,
                From = infoModel.From,
                ThreadCarrierTitle = infoModel.ThreadCarrierTitle,
                ThreadVehicle = infoModel.ThreadVehicle,
                ThreadNumber = infoModel.ThreadNumber,
                Departure = infoModel.Departure,
                To = infoModel.To,
                Image1 = picture
            };
        }

        private bool IsFlightEqualOfFavoriteModel(FavoriteModel model)
        {
            return model.CountryFrom == _dataOfFlightsModel.CountryFrom && model.CityFrom == _dataOfFlightsModel.CityFrom
                   && model.CountryTo == _dataOfFlightsModel.CountryTo && model.CityTo == _dataOfFlightsModel.CityTo;
        }
    }
}

