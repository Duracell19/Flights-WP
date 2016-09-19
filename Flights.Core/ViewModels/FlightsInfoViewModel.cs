using Flights.Infrastructure;
using Flights.Models;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Flights.Core.ViewModels
{
    public class FlightsInfoViewModel : MvxViewModel
    {
        readonly IWPHardwareButtonEvents _platformEvents;
        string[] namesOfFields = {"Arrival terminal", "Carrier title", "Vehicle", "Number", "Duration",
            "Departure", "Arrival", "To", "From" };
        ObservableCollection<InfoAboutFlyModel> infoFlyList = new ObservableCollection<InfoAboutFlyModel>();

        public FlightsInfoViewModel(IWPHardwareButtonEvents platformEvents)
        {
            _platformEvents = platformEvents;
            _platformEvents.BackButtonPressed += BackButtonPressed;
        }
    
        public ObservableCollection<InfoAboutFlyModel> InfoFlyList
        {
            get
            {
                return infoFlyList;
            }
            set
            {
                infoFlyList = value;
                RaisePropertyChanged(() => this.InfoFlyList);
            }
        }
       
        public void Init(FlyInfoShowModel flightsItem)
        {
            string[] info = new string[namesOfFields.Length];
            info[0] = (flightsItem.ArrivalTerminal != null) ? flightsItem.ArrivalTerminal : "No info";
            info[1] = (flightsItem.ThreadCarrierTitle != null) ? flightsItem.ThreadCarrierTitle : "No info"; 
            info[2] = (flightsItem.ThreadVehicle != null) ? flightsItem.ThreadVehicle : "No info"; 
            info[3] = (flightsItem.ThreadNumber != null) ? flightsItem.ThreadNumber : "No info"; 
            info[4] = (flightsItem.Duration != null) ? flightsItem.Duration : "No info"; 
            info[5] = (flightsItem.Departure != null) ? flightsItem.Departure : "No info"; 
            info[6] = (flightsItem.Arrival != null) ? flightsItem.Arrival : "No info"; 
            info[7] = (flightsItem.To != null) ? flightsItem.To : "No info"; 
            info[8] = (flightsItem.From != null) ? flightsItem.From : "No info"; 
            for (int i = 0; i < namesOfFields.Length; i++)
            {
                InfoFlyList.Add(new InfoAboutFlyModel
                {
                    StaticInformation = namesOfFields[i],
                    Information = info[i]
                });
            }
        }

        void BackButtonPressed(object sender, EventArgs e)
        {
            Close(this);
            _platformEvents.BackButtonPressed -= BackButtonPressed;
        }

        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
    }
}