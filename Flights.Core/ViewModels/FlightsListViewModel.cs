﻿using Flights.Infrastructure;
using Flights.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.File;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class FlightsListViewModel : MvxViewModel
    {
        private readonly IJsonConverterService _jsonConverter;
        private readonly IMvxFileStore _fileStore;
        private readonly IFlightsService _flightsService;
        private int count = 0;
        private DataOfFlightsModel _dataOfFlightsModel;
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
            IJsonConverterService jsonConverter, 
            IMvxFileStore fileStore,
            IFlightsService flightsService)
        {
            _jsonConverter = jsonConverter;
            _flightsService = flightsService;
            _fileStore = fileStore;
            _dataOfFlightsModel = new DataOfFlightsModel();
            _addFavorite = new ObservableCollection<FavoriteModel>();
            _favoriteList = new ObservableCollection<FavoriteModel>();
            _flightsList = new ObservableCollection<FlyInfoShowModel>();
            
            BackCommand = new MvxCommand(() => ShowViewModel<MainPageViewModel>(_dataOfFlightsModel));
            ShowFlightDetailsCommand = new MvxCommand<FlyInfoShowModel>(ShowFlyDetails);
            ShowHelpInformationCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
            ShowAboutInforamtionCommand = new MvxCommand(() => ShowViewModel<AboutViewModel>());
            AddToFavoritesCommand = new MvxCommand(AddToFavorites);
        }

        public void Init(DataOfFlightsModel dataOfFlightsModel) 
        {
            _dataOfFlightsModel = dataOfFlightsModel; 
            _dataOfFlightsModel.CitiesFrom = _jsonConverter.Deserialize<List<string>>(_dataOfFlightsModel.CitiesF);
            _dataOfFlightsModel.CitiesTo = _jsonConverter.Deserialize<List<string>>(_dataOfFlightsModel.CitiesT);
            _dataOfFlightsModel.IataFrom = _jsonConverter.Deserialize<List<string>>(_dataOfFlightsModel.IataF);
            _dataOfFlightsModel.IataTo = _jsonConverter.Deserialize<List<string >>(_dataOfFlightsModel.IataT);
            _favoriteList = Load<ObservableCollection<FavoriteModel>>(Defines.FAVORITE_LIST_FILE_NAME);
            ShowFlightsAsync();
        }

        private void AddToFavorites()
        {
            AddFavorite();
            Save(Defines.FAVORITE_LIST_FILE_NAME, _favoriteList);
            IsFlightAddedToFavorite = false;
        }

        private void ShowFlyDetails(FlyInfoShowModel c)
        {
            ShowViewModel<FlightsInfoViewModel>(FlightsList.ElementAt(c.id));
        }

        private void AddFavorite()
        {
            _addFavorite = _favoriteList;
            _addFavorite.Add(new FavoriteModel
            {
                CitiesFrom = _dataOfFlightsModel.CitiesFrom,
                CitiesTo = _dataOfFlightsModel.CitiesTo,
                CityFrom = _dataOfFlightsModel.CityFrom,
                CityTo = _dataOfFlightsModel.CityTo,
                CountryFrom = _dataOfFlightsModel.CountryFrom,
                CountryTo = _dataOfFlightsModel.CountryTo,
                IataFrom = _dataOfFlightsModel.IataFrom,
                IataTo = _dataOfFlightsModel.IataTo,
                Image1 = "ms-appx:///Assets/favorite.png",
                Image2 = "ms-appx:///Assets/direction.png"
            });
        }

        private async void ShowFlightsAsync()
        {
            FlyInfoModel[] _flyInfoOneWayModel = await _flightsService.ConfigurationOfFlights(_dataOfFlightsModel, _dataOfFlightsModel.DateOneWay, false);
            FlyInfoModel[] _flyInfoReturnModel = await _flightsService.ConfigurationOfFlights(_dataOfFlightsModel, _dataOfFlightsModel.DateReturn, true);
            await GenerateFlightsListAsync(_flyInfoOneWayModel, false);
            if (_dataOfFlightsModel.ReturnWay == true)
            {
                await GenerateFlightsListAsync(_flyInfoReturnModel, true);
            }
            IsLoadProcess = false;
            if (_favoriteList != null)
            {
                IsFlightAddedToFavorite = !(_favoriteList.Any(IsFlightEqualOfFavoriteModel));
            }
            else
            {
                _favoriteList = new ObservableCollection<FavoriteModel>();
            }
        }

        private async Task GenerateFlightsListAsync(FlyInfoModel[] flyInfoModel, bool returnWay)
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

        private bool IsFlightEqualOfFavoriteModel(FavoriteModel model)
        {
            return model.CountryFrom == _dataOfFlightsModel.CountryFrom && model.CityFrom == _dataOfFlightsModel.CityFrom
                   && model.CountryTo == _dataOfFlightsModel.CountryTo && model.CityTo == _dataOfFlightsModel.CityTo;
        }

        private T Load<T>(string fileName)
        {
            string txt;
            T result = default(T);
            if(_fileStore.TryReadTextFile(fileName, out txt))
            {
                return _jsonConverter.Deserialize<T>(txt);
            }
            return result;
        }

        private void Save(string fileName, object obj)
        {
            _fileStore.WriteFile(fileName, _jsonConverter.Serialize(_favoriteList));
        }
    }
}