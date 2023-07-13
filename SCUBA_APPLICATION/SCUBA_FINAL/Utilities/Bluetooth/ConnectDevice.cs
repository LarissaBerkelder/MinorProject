using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUBA_FINAL.Utilities.Bluetooth
{
    public class ConnectDevice
    {
       
        // Bluetooth adapter
        private readonly IAdapter _adapter;

        // Bluetooth device 
        private readonly IDevice _device;
        public ConnectDevice(IDevice device)
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            _device = device;
        }

        public async Task Connect()
        {
            var connectParameters = new ConnectParameters(false, true);
            await _adapter.ConnectToDeviceAsync(_device, connectParameters);
        }

    }
}
