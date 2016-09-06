using Flights.Models;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class FlightsInfoViewModel : MvxViewModel
    {
        string textArrivalTerminal = "No info";
        public string TextArrivalTerminal
        {
            get
            {
                return textArrivalTerminal;
            }
            set
            {
                textArrivalTerminal = value;
                RaisePropertyChanged(() => this.TextArrivalTerminal);
            }
        }
        string textCarrierTitle = "No info";
        public string TextCarrierTitle
        {
            get
            {
                return textCarrierTitle;
            }
            set
            {
                textCarrierTitle = value;
                RaisePropertyChanged(() => this.TextCarrierTitle);
            }
        }
        string textVehicle = "No info";
        public string TextVehicle
        {
            get
            {
                return textVehicle;
            }
            set
            {
                textVehicle = value;
                RaisePropertyChanged(() => this.TextVehicle);
            }
        }
        string textNumber= "No info";
        public string TextNumber
        {
            get
            {
                return textNumber;
            }
            set
            {
                textNumber = value;
                RaisePropertyChanged(() => this.TextNumber);
            }
        }
        string textDuration = "No info";
        public string TextDuration
        {
            get
            {
                return textDuration;
            }
            set
            {
                textDuration = value;
                RaisePropertyChanged(() => this.TextDuration);
            }
        }
        string textArrival = "No info";
        public string TextArrival
        {
            get
            {
                return textArrival;
            }
            set
            {
                textArrival= value;
                RaisePropertyChanged(() => this.TextArrival);
            }
        }
        string textDeparture = "No info";
        public string TextDeparture
        {
            get
            {
                return textDeparture;
            }
            set
            {
                textDeparture = value;
                RaisePropertyChanged(() => this.TextDeparture);
            }
        }
        string textTo = "No info";
        public string TextTo
        {
            get
            {
                return textTo;
            }
            set
            {
                textTo = value;
                RaisePropertyChanged(() => this.TextTo);
            }
        }
        string textFrom = "No info";
        public string TextFrom
        {
            get
            {
                return textFrom;
            }
            set
            {
                textFrom = value;
                RaisePropertyChanged(() => this.TextFrom);
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }

        public void Init(FlyInfoShowModel flightsItem)
        {
            if (flightsItem.ArrivalTerminal != null)
                TextArrivalTerminal = flightsItem.ArrivalTerminal;
            if (flightsItem.ThreadCarrierTitle != null)
                TextCarrierTitle = flightsItem.ThreadCarrierTitle;
            if (flightsItem.ThreadVehicle != null)
                TextVehicle = flightsItem.ThreadVehicle;
            if (flightsItem.ThreadNumber != null)
                TextNumber = flightsItem.ThreadNumber;
            TextDuration = flightsItem.Duration;
            TextArrival = flightsItem.Arrival;
            TextDeparture = flightsItem.Departure;
            TextTo = flightsItem.ToTitle;
            TextFrom = flightsItem.FromTitle;
        }
    }
}