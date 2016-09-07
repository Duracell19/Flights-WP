using Flights.Core.Commands;
using Flights.Infrastructure;
using Flights.Models;
using Flights.Services.DataModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Flights.Core.ViewModels
{
    public class FlightsListViewModel : MvxViewModel
    {
        MainPageModel mainPageModel = new MainPageModel();
        ObservableCollection<FavoriteModel> addFavorite= new ObservableCollection<FavoriteModel>();
        ObservableCollection<FavoriteModel> favoriteList = new ObservableCollection<FavoriteModel>();
        FlyInfoModel[] flyInfoOneWayModel;
        FlyInfoModel[] flyInfoReturnModel;
        FlightsService flightsService;
        readonly IHttpService _httpService;
        readonly IDeserializService _deserializService;

        public FlightsListViewModel(IHttpService httpService, IDeserializService deserializService)
        {
            _httpService = httpService;
            flightsService = new FlightsService(_httpService);
            _deserializService = deserializService;
        }
        
        bool isActiveProcess = true;
        public bool IsActiveProcess
        {
            get
            {
                return isActiveProcess;
            }
            set
            {
                isActiveProcess = value;
                RaisePropertyChanged(() => this.IsActiveProcess);
            }
        }
        string textLoad = "Please wait...";
        public string TextLoad
        {
            get
            {
                return textLoad;
            }
            set
            {
                textLoad = value;
                RaisePropertyChanged(() => this.TextLoad);
            }
        }
        bool isEnabledFavorite;
        public bool IsEnabledFavorite
        {
            get
            {
                return isEnabledFavorite;
            }
            set
            {
                isEnabledFavorite = value;
                RaisePropertyChanged(() => this.IsEnabledFavorite);
            }
        }

        ObservableCollection<FlyInfoShowModel> flightsList = new ObservableCollection<FlyInfoShowModel>();
        public ObservableCollection<FlyInfoShowModel> FlightsList
        {
            get
            {
                return flightsList;
            }
            set
            {
                flightsList = value;
                RaisePropertyChanged(() => this.FlightsList);
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<MainPageViewModel>(mainPageModel));
            }
        }
        private MvxCommand<FlyInfoShowModel> selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get { return selectionChangedCommand ?? (selectionChangedCommand = new MvxCommand<FlyInfoShowModel>(c => this.ShowFlyInfo(c))); }
        }
        public void ShowFlyInfo(FlyInfoShowModel c)
        {
            ShowViewModel<FlightsInfoViewModel>(FlightsList[c.id]);
        }
        private ICommand helpCommand;
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
        private ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                return aboutCommand
                    ?? (aboutCommand = new ActionCommand(() =>
                    {
                        ShowViewModel<AboutViewModel>();
                    }));
            }
        }
        private ICommand favoriteCommand;
        public ICommand FavoriteCommand
        {
            get
            {
                return favoriteCommand
                    ?? (favoriteCommand = new ActionCommand(() =>
                    {
                        AddFavorite();
                        var fileService = Mvx.Resolve<IMvxFileStore>();
                        fileService.WriteFile("favoriteList.xml", (stream) =>
                                                                 {
                                                                     var serializer = new XmlSerializer(typeof(ObservableCollection<FavoriteModel>));
                                                                     serializer.Serialize(stream, addFavorite);
                                                                 });

                        IsEnabledFavorite = false;
                    }));
            }
        }
        
        public void Init(MainPageModel _mainPageModel)
        {
            mainPageModel = _mainPageModel;
            mainPageModel.CitiesFrom = _deserializService.Deserializ(mainPageModel.CitiesF);
            mainPageModel.CitiesTo = _deserializService.Deserializ(mainPageModel.CitiesT);
            mainPageModel.IataFrom = _deserializService.Deserializ(mainPageModel.IataF);
            mainPageModel.IataTo = _deserializService.Deserializ(mainPageModel.IataT);

            var fileService = Mvx.Resolve<IMvxFileStore>();
            fileService.TryReadBinaryFile("favoriteList.xml", (inputStream) =>
            {
                return LoadFrom(inputStream);
            });

            GenerateFlightsList();
        }

        private async void GenerateFlightsList()
        {
            int count = mainPageModel.IataFrom.Length * mainPageModel.IataTo.Length;
            flyInfoOneWayModel = new FlyInfoModel[count];
            flyInfoReturnModel = new FlyInfoModel[count];
            int valueOneWay = -1;
            int valueReturn = -1;
            for (int i = 0; i < mainPageModel.IataFrom.Length; i++)
            {
                for (int j = 0; j < mainPageModel.IataTo.Length; j++)
                {
                    valueOneWay++;
                    flyInfoOneWayModel[valueOneWay] = new FlyInfoModel();
                    flyInfoOneWayModel[valueOneWay] = await flightsService.GetFlight(mainPageModel.IataFrom[i], mainPageModel.IataTo[j], mainPageModel.DateOneWay);
                    if (flyInfoOneWayModel[valueOneWay] == null)
                        valueOneWay--;
                }
            }
            if (mainPageModel.ReturnWay)
            {
                for (int i = 0; i < mainPageModel.IataTo.Length; i++)
                {
                    for (int j = 0; j < mainPageModel.IataFrom.Length; j++)
                    {
                        valueReturn++;
                        flyInfoReturnModel[valueReturn] = new FlyInfoModel();
                        flyInfoReturnModel[valueReturn] = await flightsService.GetFlight(mainPageModel.IataTo[i], mainPageModel.IataFrom[j], mainPageModel.DateReturn);
                        if (flyInfoReturnModel[valueReturn] == null)
                            valueReturn--;
                    }
                }
            }
            ShowFlights(valueOneWay, valueReturn);
        }
        private void ShowFlights(int valueOneWay, int valueReturn)
        {
            int count = 0;
            if (valueOneWay != -1)
            {
                for (int i = 0; i < valueOneWay + 1; i++)
                {
                    for (int j = 0; j < flyInfoOneWayModel[i].Count; j++)
                    {
                        FlightsList.Add(new FlyInfoShowModel
                        {
                            Arrival = flyInfoOneWayModel[i].Arrival[j],
                            Duration = flyInfoOneWayModel[i].Duration[j],
                            ArrivalTerminal = flyInfoOneWayModel[i].ArrivalTerminal[j],
                            FromTitle = flyInfoOneWayModel[i].FromTitle[j],
                            ThreadCarrierTitle = flyInfoOneWayModel[i].ThreadCarrierTitle[j],
                            ThreadVehicle = flyInfoOneWayModel[i].ThreadVehicle[j],
                            ThreadNumber = flyInfoOneWayModel[i].ThreadNumber[j],
                            Departure = flyInfoOneWayModel[i].Departure[j],
                            ToTitle = flyInfoOneWayModel[i].ToTitle[j],
                            Image1 = "ms-appx:///Assets/fly.png",
                            Image2 = "ms-appx:///Assets/direction.png",
                            id = count
                        });
                        count++;
                    }
                }
                if (mainPageModel.ReturnWay && valueReturn != -1)
                {
                    for (int i = 0; i < valueReturn + 1; i++)
                    {
                        for (int j = 0; j < flyInfoReturnModel[i].Count; j++)
                        {
                            FlightsList.Add(new FlyInfoShowModel
                            {
                                Arrival = flyInfoReturnModel[i].Arrival[j],
                                Duration = flyInfoReturnModel[i].Duration[j],
                                ArrivalTerminal = flyInfoReturnModel[i].ArrivalTerminal[j],
                                FromTitle = flyInfoReturnModel[i].FromTitle[j],
                                ThreadCarrierTitle = flyInfoReturnModel[i].ThreadCarrierTitle[j],
                                ThreadVehicle = flyInfoReturnModel[i].ThreadVehicle[j],
                                ThreadNumber = flyInfoReturnModel[i].ThreadNumber[j],
                                Departure = flyInfoReturnModel[i].Departure[j],
                                ToTitle = flyInfoReturnModel[i].ToTitle[j],
                                Image1 = "ms-appx:///Assets/fly_return.png",
                                Image2 = "ms-appx:///Assets/direction.png",
                                id = count
                            });
                            count++;
                        }
                    }
                }
            }
            else
            {
                FlightsList.Add(new FlyInfoShowModel
                {
                    Arrival = "",
                    Duration = "",
                    ArrivalTerminal = "",
                    FromTitle = "There are no flights..",
                    ThreadCarrierTitle = "",
                    ThreadVehicle = "",
                    ThreadNumber = "",
                    Departure = "",
                    ToTitle = "",
                    Image1 = "",
                    Image2 = "",
                    id = count
                });
            }
            IsActiveProcess = false;
            TextLoad = "";
            IsEnabledFavorite = IsTheSame();
        }
        private void AddFavorite()
        {
            addFavorite = favoriteList;
            addFavorite.Add(new FavoriteModel
            {
                CitiesFrom = mainPageModel.CitiesFrom,
                CitiesTo = mainPageModel.CitiesTo,
                CityFrom = mainPageModel.CityFrom,
                CityTo = mainPageModel.CityTo,
                CountryFrom = mainPageModel.CountryFrom,
                CountryTo = mainPageModel.CountryTo,
                IataFrom = mainPageModel.IataFrom,
                IataTo = mainPageModel.IataTo,
                Image1 = "ms-appx:///Assets/favorite.png",
                Image2 = "ms-appx:///Assets/direction.png"
            });
        }
        private bool IsTheSame()
        {
            int value = 0;
            foreach (FavoriteModel f in favoriteList)
            {
                if (f.CountryFrom.Equals(mainPageModel.CountryFrom) && f.CityFrom.Equals(mainPageModel.CityFrom) 
                    && f.CountryTo.Equals(mainPageModel.CountryTo) && f.CityTo.Equals(mainPageModel.CityTo))
                {
                    value++;
                }
            }
            if (value != 0)
            {
                return false;
            }
            else
                return true;
        }
        private bool LoadFrom(Stream inputStream)
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