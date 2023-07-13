using Matcha.BackgroundService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SCUBA_FINAL.Utilities.Service;

namespace SCUBA_FINAL.Services
{
    public class GetMessageService : IPeriodicTask
    {
        public TimeSpan Interval { get; set; }

        public GetMessageService(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }
        public async Task<bool> StartJob()
        {
            var message_bytes = await ((App)Application.Current).Characteristic.ReadAsync();
            string Message = Encoding.UTF8.GetString(message_bytes);
            if (Message[0] == '$') new ProcessMessage(Message);
            return true; 
        }

    }
}
