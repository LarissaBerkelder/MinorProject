using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SCUBA_FINAL.Utilities.Bluetooth
{
    public class GetServiceAndCharacterics
    {
        private IDevice Device { get; set; }
        private IService Service { get; set; }
        public bool Succes { get; set; }
        public GetServiceAndCharacterics(IDevice device)
        {
            Device = device;
            Succes = false;
        }

        public async Task GetService()
        {
            var services = await Device.GetServicesAsync();
            if (services != null)
            {
                foreach (var service in services)
                {
                    if (service.Name.Contains("Unknown Service"))
                    {
                        Service = service;
                        await GetCharacteristic();
                    }
                }
            }
        }

        public async Task GetCharacteristic()
        {
            var characteristics = await Service.GetCharacteristicsAsync();
            if (characteristics != null)
            {
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.CanRead && characteristic.CanWrite)
                    {
                        // Saving characteristic global in application
                        ((App)Application.Current).Characteristic = characteristic;
                        Succes = true;
                    }
                }
            }
        }
    }
}
