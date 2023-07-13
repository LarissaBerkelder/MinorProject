using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace SCUBA_FINAL.Utilities.Map
{
    public class CreatePositionMap
    {
        public Pin CreatePin(string location)
        {
            string[] coordinate_parts = location.Split(',');
           
            var position = new Position(
                double.Parse(coordinate_parts[0]),
                double.Parse(coordinate_parts[1]));

            var pin = new Pin
            {
                Position = position,
                Label = "Location diver"
            };

            return pin;
        }

        public Pin CreatePinDevice(double lat, double lon)
        {
            var position = new Position(lat, lon);
            var pin = new Pin
            {
                Position = position,
                Label = "Location device"
            };

            return pin;
        }
    }
}
