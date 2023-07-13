using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SCUBA_FINAL.Utilities.Email
{
    public class EmailHelper
    {
        public string EmailData { get; set; }
        public async Task GetLocationData()
        {
            // Get data from the database
            var data_database = await App.GPB_Database.GetDataAsync();
            string data = "Data from the dive: \n\n";
            if(data_database != null)
            {
                foreach(var item in data_database)
                {
                    data += item.MessageReceived + " " + item.Date.ToString() + "\n";
                }
            }
            EmailData = data;   
        }

        public async Task SendEmail(string subject, List<string> recipients)
        {
            // Retrieving the data from the database
            await GetLocationData();
            // Creating email message
            var message = new EmailMessage
            {
                Subject = subject,
                Body = EmailData,
                To = recipients,

            };
            try
            {
                await Xamarin.Essentials.Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                Debug.WriteLine(fbsEx);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                Debug.WriteLine(ex);
            }
        }
    }
}
