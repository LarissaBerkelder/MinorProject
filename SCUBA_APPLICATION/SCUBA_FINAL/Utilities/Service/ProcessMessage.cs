using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SCUBA_FINAL.Models;

namespace SCUBA_FINAL.Utilities.Service
{
    public class ProcessMessage
    {
        private string Type { get; set; }
        private string Value { get; set; }  

        public ProcessMessage(string message)
        {
            Process(message);
        }

        public void Process(string message)
        {
            message = message.Substring(2);
            string[] message_parts = message.Split('|');
            Type = message_parts[0];
            Value = message_parts[1];

            switch (Type)
            {
                case "EMR":
                    if (CheckPrev(((App)Application.Current).PrevEMR)) SaveEMR();
                    break;
                case "MSG":
                    if (CheckPrev(((App)Application.Current).PrevMSG)) SaveMSG();
                    break;
                case "GPB":
                    if (CheckPrev(((App)Application.Current).PrevGPB)) SaveGPB();
                    break;
                default:
                    break; 
            }
        }

        public bool CheckPrev(string prevMessage)
        {
            // Check if the message is new if the message is a new message save it to the database
            if (prevMessage.Equals(Value))
            {
                // Message is not a new message
                return false;
            }
            else return true;
        }

        public async void SaveEMR()
        {
            // Saving the message to the database 
            await App.EMR_Database.SaveDataAsync(new EMR_Model
            {
                MessageReceived = Value,
                Date = DateTime.Now
            });
            // Saving the message as global for checking 
            ((App)Application.Current).PrevEMR = Value;
            // Adding to the notification 
            ((App)Application.Current).EMR_Notification++;
            MessagingCenter.Send<object, string>(this, "Notification", Value);
        }

        public async void SaveMSG()
        {
            // Saving the message to the database 
            await App.MSG_Database.SaveDataAsync(new MSG_Model
            {
                MessageReceived = Value,
                Date = DateTime.Now
            });
            // Saving the message as global for checking 
            ((App)Application.Current).PrevMSG = Value;
            // Adding to the notification 
            ((App)Application.Current).MSG_Notification++;
            MessagingCenter.Send<object, string>(this, "Notification", "MSG");
        }

        public async void SaveGPB()
        {
            Debug.WriteLine("Saving GPB");
            // Saving the message to the database 
            await App.GPB_Database.SaveDataAsync(new GPB_Model
            {
                MessageReceived = Value,
                Date = DateTime.Now
            });
            // Saving the message as global for checking 
            ((App)Application.Current).PrevGPB = Value;
            MessagingCenter.Send<object, string>(this, "Notification", "GPB");
        }
    }
}
