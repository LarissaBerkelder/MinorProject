using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SCUBA_FINAL.ViewModels;

namespace SCUBA_FINAL.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
            BindingContext = new ChatViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as ChatViewModel;
            viewModel?.OnAppearingCommand?.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var viewModel = BindingContext as ChatViewModel;
            viewModel?.OnDisappearingCommand?.Execute(null);
        }
    }
}