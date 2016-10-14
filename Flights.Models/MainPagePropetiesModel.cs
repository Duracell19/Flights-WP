using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Flights.Models
{
    public class MainPagePropetiesModel
    {
        public bool IsEnabledCityFrom { get; set; } 
        public bool IsEnabledCityTo { get; set; } 
        public bool IsCheckedOneWay { get; set; } 
        public bool IsCheckedReturn { get; set; } 
        public bool IsEnabledDateReturn { get; set; } 
        public bool IsEnabledButtonFind { get; set; } 
        public bool VisibilityClear { get; set; } 
        public bool VisibilityRefresh { get; set; }
        public string PlaceholderTextCityFrom { get; set; } 
        public string PlaceholderTextCityTo { get; set; } 
        public string TextCountryFrom { get; set; } 
        public string TextCountryTo { get; set; } 
        public string TextCityFrom { get; set; } 
        public string TextCityTo { get; set; } 
        public string TextAbout { get; set; }
        public DateTimeOffset DateOneWay { get; set; } 
        public DateTimeOffset DateReturn { get; set; } 
        public int PivotNumber { get; set; }
        public List<string> CountriesFrom { get; set; }
        public List<string> CountriesTo { get; set; }
        public List<string> CitiesFrom { get; set; }
        public List<string> CitiesTo { get; set; }
        public ObservableCollection<FavoriteModel> FavoriteList { get; set; }
    }
}
