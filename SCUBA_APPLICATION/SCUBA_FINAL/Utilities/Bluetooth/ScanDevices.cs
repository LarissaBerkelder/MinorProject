using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SCUBA_FINAL.Utilities.Bluetooth
{
    public class ScanDevices
    {
        private readonly IAdapter _adapter;
        private List<IDevice> _bluetooth_devices;
        public List<IDevice> BluetoothDevices
        {
            get { return _bluetooth_devices; }
            set { _bluetooth_devices = value; }
        }

        public ScanDevices()
        {
            BluetoothDevices = new List<IDevice>();

            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += (s, a) =>
            {
                BluetoothDevices.Add(a.Device);
            };
        }

        public async Task StartScanning()
        {
            // Ask for location permission needed to connect to bluetooth device
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            // Start scanning for bluetooth devices
            await _adapter.StartScanningForDevicesAsync();
        }

    }
}
