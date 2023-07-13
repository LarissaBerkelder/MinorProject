using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

using SCUBA_FINAL.Utilities.Email;
using System.Diagnostics;
using SCUBA_FINAL.Views;

namespace SCUBA_FINAL.ViewModels
{
    public class SaveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Send button 
        private Command sendCommand;
        public Command SendCommand
        {
            get { return sendCommand; }
            set
            {
                sendCommand = value;
                OnPropertyChanged(nameof(SendCommand));
            }
        }

        // Editor 
        private string subject;
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        private string recipient;
        public string Recipient
        {
            get { return recipient; }
            set
            {
                recipient = value;
                OnPropertyChanged(nameof(Recipient));
            }
        }

        private readonly string DefaultRecipient = "scupaminor@gmail.com";

        // Utilities
        ValidateEmail validate = new ValidateEmail();
        EmailHelper email = new EmailHelper();
       
        public SaveViewModel()
        {
            SendCommand = new Command(SendButtonClicked);
        }

        private async void SendButtonClicked()
        {
            // Reading the user input for the email subject if null use default
            string EmailSubject = "Dive Information";
            if(Subject != null)
            {
                EmailSubject = Subject;
                Subject = string.Empty;
            }

            // Creating list for recipients 
            List<string> EmailRecipients = new List<string>();
            // Adding the default 
            EmailRecipients.Add(DefaultRecipient);

            // Reading the user input for recipient
            if(Recipient != null)
            {
                // Validate email adress
                if (validate.IsValidEmail(Recipient))
                {
                    EmailRecipients.Add(Recipient);
                    Recipient = string.Empty;
                }
                else
                {
                    Debug.WriteLine("Email is not valid");
                    Recipient = string.Empty;
                }
            }

            await email.SendEmail(EmailSubject, EmailRecipients);

            // Go back to the menu page
            await App.Current.MainPage.Navigation.PushAsync(new MenuPage());
        }
     
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
