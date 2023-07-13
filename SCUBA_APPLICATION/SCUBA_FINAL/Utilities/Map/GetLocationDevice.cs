using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SCUBA_FINAL.Utilities.Map
{
    public class GetLocationDevice
    {
        public double Lat { get; set; }
        public double Lon { get; set; }

        async Task<Location> GetCurrentLocationAsync()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(request);
            return location;
        }

        public async Task GetLocationDeviceAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    // Handle permission denied
                    Debug.WriteLine("Permission denied");
                    return;
                }
            }

            var location = await GetCurrentLocationAsync();
            if (location != null)
            {
                Lat = location.Latitude;
                Lon = location.Longitude;

            }
        }

    }
}
