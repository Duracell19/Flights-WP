using Flights.Models;
using MvvmCross.Core.ViewModels;

namespace Flights.Core.ViewModels
{
    public class FlightsInfoViewModel : MvxViewModel
    {
        private FlyInfoShowModel infoFlyList;

        public FlyInfoShowModel InfoFlyList
        {
            get { return infoFlyList; }
        }

        public void Init(FlyInfoShowModel flightsItem)
        {
            infoFlyList = flightsItem;
            RaisePropertyChanged(() => InfoFlyList);
        }
    }
}