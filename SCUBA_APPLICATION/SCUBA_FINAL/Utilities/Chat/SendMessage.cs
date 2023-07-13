using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SCUBA_FINAL.Utilities.Chat
{
    public class SendMessage
    {
        public async void SendMessageAsync(string message, string type)
        {
            string Sendingmessage = $"|{type}|{message}|"; 
            // Transforming the message from a string to bytes
            byte[] SendMessage = Encoding.ASCII.GetBytes(Sendingmessage);

            ICharacteristic Characteristic = ((App)Application.Current).Characteristic;

            var succes = await Characteristic.WriteAsync(SendMessage);

            // If not succesfull try sending the message again
            Debug.WriteLine($"Sending message: {Sendingmessage} succesfull: {succes}");
            while (!succes)
            {
                Debug.WriteLine($"Sending message: {Sendingmessage} succesfull: {succes}");
                succes = await Characteristic.WriteAsync(SendMessage);

            }
        }
    }
}
