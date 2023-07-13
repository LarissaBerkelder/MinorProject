using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using SCUBA_FINAL.Utilities.Bluetooth;
using SCUBA_FINAL.Views;

namespace SCUBA_FINAL.ViewModels
{
    public class ConnectBluetoothViewModel
    {
        // The device name of the ESP32 
        public readonly string DeviceName = "ESP32";

        private IDevice Device { get; set; }

        public ConnectBluetoothViewModel()
        {
            ConnectBluetooth();
        }

        public async void ConnectBluetooth()
        {
            // Scan for devices
            ScanDevices scanDevices = new ScanDevices();
            await scanDevices.StartScanning();
            try
            {
                // Select the ESP32 device
                Device = scanDevices.BluetoothDevices.FirstOrDefault(d => d.Name == DeviceName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred {ex}");
            }

            // Connect to the device 
            ConnectDevice connect = new ConnectDevice(Device);
            await connect.Connect();
            GetServiceAndCharacterics get_s_c = new GetServiceAndCharacterics(Device);
            await get_s_c.GetService();

            // Check if the device is connected otherwise re-run the function
            if (Device.State == DeviceState.Connected && get_s_c.Succes)
            {
                await App.Current.MainPage.Navigation.PushAsync(new MenuPage());
            }
            else
            {
                ConnectBluetooth();
            }
        }


            
    }
}

