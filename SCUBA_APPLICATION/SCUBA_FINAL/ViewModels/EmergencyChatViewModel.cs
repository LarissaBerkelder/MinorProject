using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

using SCUBA_FINAL.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SCUBA_FINAL.Utilities.Chat;
using System.Linq;

namespace SCUBA_FINAL.ViewModels
{
    public class EmergencyChatViewModel : BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Setting the commands for when the page appears or dissapears 
        public Command OnAppearingCommand => new Command(OnAppearing);
        public Command OnDisappearingCommand => new Command(OnDisappearing);

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
        private string _editorText;
        public string EditorText
        {
            get { return _editorText; }
            set
            {
                _editorText = value;
                OnPropertyChanged(nameof(EditorText));
            }
        }

        // Collection view list
        private ObservableCollection<ChatModel> _messagesList;
        public ObservableCollection<ChatModel> MessagesList
        {
            get { return _messagesList; }
            set
            {
                _messagesList = value;
                OnPropertyChanged(nameof(MessagesList));
            }
        }

        // Utilities
        private CreateMessage createMessage = new CreateMessage();
        private SendMessage SendMessage = new SendMessage();

        private void OnAppearing()
        {
            // Get messages from database
            ShowChat();

            // Subscribe to notification messages
            MessagingCenter.Subscribe<object, string>(this, "Notification", async (sender, arg) =>
            {
                if (arg != "MSG" && arg != "GPB")
                {
                    // Update messages
                    ShowChat();
                }
            });
        }

        private void OnDisappearing()
        {
            // Set notification count to zero 
            ((App)Application.Current).EMR_Notification = 0;
            // Unsubsribe 
            MessagingCenter.Unsubscribe<object, string>(this, "Notification");
        }


        public EmergencyChatViewModel()
        {
            SendCommand = new Command(SendButtonClicked);
            MessagesList = new ObservableCollection<ChatModel>();
        }

        private async void ShowChat()
        {
            var messages = await App.EMR_Database.GetDataAsync();

            if (messages != null)
            {
                // Clear the list to create an updated list
                MessagesList.Clear();
                // Going over all the messages saved in the database
                foreach (var message in messages)
                {
                    if (message.MessageReceived != null)
                    {
                        MessagesList.Add(createMessage.CreateMessageReceived(message.MessageReceived));
                    }
                    else if (message.MessageSend != null)
                    {
                        MessagesList.Add(createMessage.CreateMessageSend(message.MessageSend));
                    }
                }
                MessagesList = new ObservableCollection<ChatModel>(_messagesList.OrderBy(m => m.Date));

            }
        }

        private async void SendButtonClicked()
        {
            string userInput = EditorText;
            // Saving the message in the database
            await App.EMR_Database.SaveDataAsync(new EMR_Model { MessageSend = userInput, Date = DateTime.Now });
            // sending the message to the ESP32
            SendMessage.SendMessageAsync(userInput, "EMR");
            // Clear the entry
            EditorText = string.Empty;
            // Update the chatList
            ShowChat();
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
