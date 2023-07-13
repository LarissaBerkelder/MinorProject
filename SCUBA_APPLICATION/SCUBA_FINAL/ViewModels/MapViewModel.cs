using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

using SCUBA_FINAL.Utilities.Map;

namespace SCUBA_FINAL.ViewModels
{
    public class MapViewModel : BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
          
        // Setting the commands for when the page appears or dissapears 
        public Command OnAppearingCommand => new Command(OnAppearing);
        public Command OnDisappearingCommand => new Command(OnDisappearing);

        private Map _myMap;
        public Map MyMap
        {
            get { return _myMap; }
            set
            {
                _myMap = value;
                OnPropertyChanged(nameof(MyMap));
            }
        }

        private Pin mapPinDiver { get; set; }
        public Pin MapPinDiver 
        {
            get { return mapPinDiver; }
            set
            {
                mapPinDiver = value;
                OnPropertyChanged(nameof(MapPinDiver));
            }
        }

        private Pin mapPinDevice { get; set; }
        public Pin MapPinDevice
        {
            get { return mapPinDevice; }
            set
            {
                mapPinDevice = value;
                OnPropertyChanged(nameof(MapPinDevice));
            }
        }

        // Utilities
        CreatePositionMap createPositionMap = new CreatePositionMap();
        GetLocationDevice getLocationDevice = new GetLocationDevice();

        private void OnAppearing()
        {
            MessagingCenter.Subscribe<object, string>(this, "Notification", (sender, arg) =>
            {
                if (arg == "GPB")
                {
                    // Update map
                    ShowDiverLocation();
                    ShowDeviceLocation();
                }
            });
        }

        private void OnDisappearing()
        {
            // Unsubsribe 
            MessagingCenter.Unsubscribe<object, string>(this, "Notification");
        }

        public MapViewModel()
        {
            MyMap = new Map();
            MyMap.MapType = MapType.Street;
            ShowDiverLocation();
            ShowDeviceLocation();

        }

        public async void ShowDeviceLocation()
        {
            // Get location from the device
            await getLocationDevice.GetLocationDeviceAsync();

            if (MapPinDevice != null)
            {
                MyMap.Pins.Remove(MapPinDevice);
            }

            MapPinDevice = createPositionMap.CreatePinDevice(getLocationDevice.Lat, getLocationDevice.Lon);
            // Adding the pin to the map 
            MyMap.Pins.Add(MapPinDevice);

        }
        public async void ShowDiverLocation()
        {
            // First clear the old location pin from the map
            if(MapPinDiver != null)
            {
                MyMap.Pins.Remove(MapPinDiver);
            }
            
            // Get the new location from the database
            var location = await App.GPB_Database.GetLastSavedDataAsync();
            // Check if location is not null 
            if(location != null)
            {
                MapPinDiver =  createPositionMap.CreatePin(location.MessageReceived);
                // Adding the pin to the map 
                MyMap.Pins.Add(MapPinDiver);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(MapPinDiver.Position, Distance.FromKilometers(1)));
            }

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
