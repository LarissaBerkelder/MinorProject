using Matcha.BackgroundService;
using Prism.Mvvm;
using SCUBA_FINAL.Models;
using SCUBA_FINAL.Services;
using SCUBA_FINAL.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SCUBA_FINAL.ViewModels
{
    public class MenuViewModel : BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Ok button emergency message
        private Command okCommand;
        public Command OkCommand
        {
            get { return okCommand; }
            set
            {
                okCommand = value;
                OnPropertyChanged(nameof(OkCommand));
            }

        }

        // Go to emergency chat button
        private Command emergencyCommand;
        public Command EmergencyCommand
        {
            get { return emergencyCommand; }
            set
            {
                emergencyCommand = value;
                OnPropertyChanged(nameof(EmergencyCommand));
            }
        }

        // Go to chat button
        private Command chatCommand;
        public Command ChatCommand
        {
            get { return chatCommand; }
            set
            {
                chatCommand = value;
                OnPropertyChanged(nameof(ChatCommand));
            }
        }

        // Go to map button
        private Command mapCommand;
        public Command MapCommand
        {
            get { return mapCommand; }
            set
            {
                mapCommand = value;
                OnPropertyChanged(nameof(MapCommand));
            }
        }

        // Go to download
        private Command downloadCommand;
        public Command DownloadCommand
        {
            get { return downloadCommand; }
            set
            {
                downloadCommand = value;
                OnPropertyChanged(nameof(DownloadCommand));
            }
        }

        // Notification chat
        private string notificationMSG;
        public string NotificationMSG
        {
            get 
            { 
                if (notificationMSG == null) 
                {
                    return "";
                }
                return notificationMSG; 
            }
            set
            {
                notificationMSG = value;
                OnPropertyChanged(nameof(NotificationMSG));
            }
        }
        // Notification emergency chat
        private string notificationEMR;
        public string NotificationEMR
        {
            get
            {
                if (notificationEMR == null)
                {
                    return "";
                }
                return notificationEMR;
            }
            set
            {
                notificationEMR = value;
                OnPropertyChanged(nameof(NotificationEMR));
            }
        }

        // Emergency message
        private string emergencyMessage;
        public string EmergencyMessage
        {
            get { return emergencyMessage; }
            set 
            {
                emergencyMessage = value;
                OnPropertyChanged(nameof(EmergencyMessage));
            }
        }

        // Visiblity frames
        private bool emergencyFrame;
        public bool EmergencyFrame
        {
            get { return emergencyFrame; }
            set
            {
                emergencyFrame = value;
                OnPropertyChanged(nameof(EmergencyFrame));
            }
        }

        private bool menuFrame;
        public bool MenuFrame
        {
            get { return menuFrame; }
            set
            {
                menuFrame = value;
                OnPropertyChanged(nameof(MenuFrame));
            }
        }

        // Setting the commands for when the page appears or dissapears 
        public Command OnAppearingCommand => new Command(OnAppearing);
        public Command OnDisappearingCommand => new Command(OnDisappearing);

        private void OnAppearing()
        {
            NotificationHandler();
            MenuFrame = true;
            EmergencyFrame = false;

            MessagingCenter.Subscribe<object, string>(this, "Notification", async (sender, arg) =>
            {
                if(arg != "MSG" && arg != "GPB")
                {
                    ShowAlertAsync(arg);
                }
                NotificationHandler();
            });

        }

        private void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<object, string>(this, "Notification");
        }

        public MenuViewModel()
        {
            // Configuring the buttons
            OkCommand = new Command(OkClicked);
            EmergencyCommand = new Command(EmergencyClicked);
            ChatCommand = new Command(ChatClicked);
            MapCommand = new Command(MapClicked);
            DownloadCommand = new Command(DownloadClicked);

            // Starting the retrieve message service for retrieving the messages from the ESP32 
            BackgroundAggregatorService.Add(() => new GetMessageService(3));
            BackgroundAggregatorService.StartBackgroundService();

        }

        public void ShowAlertAsync(string message)
        {
            MenuFrame = false;
            EmergencyFrame = true;
            EmergencyMessage = message;
        }

        private void NotificationHandler()
        {
            //NotificationMSG = ((App)Application.Current).MSG_Notification.ToString();
            if (((App)Application.Current).MSG_Notification > 0)
            {
                NotificationMSG = ((App)Application.Current).MSG_Notification.ToString();
            }
            else NotificationMSG = "";

            if (((App)Application.Current).EMR_Notification > 0)
            {
                NotificationEMR = ((App)Application.Current).EMR_Notification.ToString();
            }
            else NotificationEMR = "";
        }


        public void OkClicked()
        {
            MenuFrame = true;
            EmergencyFrame = false;
            EmergencyMessage = "";
        }

        public async void EmergencyClicked()
        {
           await App.Current.MainPage.Navigation.PushAsync(new EmergencyPage());
        }

        public async void ChatClicked()
        {
           await App.Current.MainPage.Navigation.PushAsync(new ChatPage());
        }

        public async void MapClicked()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MapPage());
        }

        public async void DownloadClicked()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SavePage());
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
