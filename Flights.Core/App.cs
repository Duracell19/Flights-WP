using Flights.Infrastructure;
using Flights.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace Flights.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            Mvx.RegisterType<ICitiesService, CitiesService>();
            Mvx.RegisterType<ICountriesService, CountriesService>();
            Mvx.RegisterType<IHttpService, HttpService>();
            Mvx.RegisterType<IIataService, IataService>();
            Mvx.RegisterType<IDateService, DateService>();
            Mvx.RegisterType<IFlightsService, FlightsService>();
            Mvx.RegisterType<ISerializXMLService, SerializXMLService>();
            Mvx.RegisterType<IDeserializXMLService, DeserializXMLService>();
            Mvx.RegisterType<IWPHardwareButtonEvents, WPHardwareButtonEvents>();
            Mvx.RegisterType<IJsonConverter, JsonConverter>();
            RegisterAppStart<ViewModels.MainPageViewModel>();
        }
    }
}
