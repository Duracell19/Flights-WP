using Flights.Infrastructure.Interfaces;
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
            Mvx.RegisterType<IFlightsService, FlightsService>();
            Mvx.RegisterType<IJsonConverter, JsonConverter>();
            Mvx.RegisterType<IFileStore, FileStore>();

            RegisterAppStart<ViewModels.MainPageViewModel>();
        }
    }
}
