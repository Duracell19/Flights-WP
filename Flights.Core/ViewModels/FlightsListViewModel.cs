using Flights.Core.Commands;
using Flights.Infrastructure;
using Flights.Models;
using Flights.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        readonly IDeserializXMLService _deserializService;
        readonly IWPHardwareButtonEvents _platformEvents;
        readonly IJsonConverter _jsonConverter;

        public FlightsListViewModel(IHttpService httpService, IDeserializXMLService deserializService, IWPHardwareButtonEvents platformEvents, IJsonConverter jsonConverter)
        {
            _httpService = httpService;
            _jsonConverter = jsonConverter;
            flightsService = new FlightsService(_httpService, _jsonConverter);
            _deserializService = deserializService;
            _platformEvents = platformEvents;
            _platformEvents.BackButtonPressed += BackButtonPressed;
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
        MvxCommand<FlyInfoShowModel> selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get { return selectionChangedCommand ?? (selectionChangedCommand = new MvxCommand<FlyInfoShowModel>(c => this.ShowFlyInfo(c))); }
        }
        public void ShowFlyInfo(FlyInfoShowModel c)
        {
            ShowViewModel<FlightsInfoViewModel>(FlightsList[c.id]);
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
        ICommand aboutCommand;
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
        ICommand favoriteCommand;
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

        async void GenerateFlightsList()
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
        void ShowFlights(int valueOneWay, int valueReturn)
        {
            if (valueOneWay != -1)
            {
                foreach (var item in flyInfoOneWayModel) 
                {
                    if (item == null)
                        continue; 
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
                            Image1 = "ms-appx:///Assets/fly.png",
                            Image2 = "ms-appx:///Assets/direction.png"
                        });
                    }
                }
                if (mainPageModel.ReturnWay && valueReturn != -1)
                { 
                    foreach (var item in flyInfoReturnModel)
                    {
                        if (item == null)
                            continue;
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
                                Image1 = "ms-appx:///Assets/fly.png",
                                Image2 = "ms-appx:///Assets/direction.png"
                            });
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
                    From = "There are no flights..",
                    ThreadCarrierTitle = "",
                    ThreadVehicle = "",
                    ThreadNumber = "",
                    Departure = "",
                    To = "",
                    Image1 = "",
                    Image2 = ""
                });
            }
            IsActiveProcess = false;
            TextLoad = "";
            IsEnabledFavorite = IsTheSame();
        }
        void AddFavorite()
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
        bool IsTheSame()
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
        void BackButtonPressed(object sender, EventArgs e)
        {
            Close(this);
            _platformEvents.BackButtonPressed -= BackButtonPressed;
        }
    }
}