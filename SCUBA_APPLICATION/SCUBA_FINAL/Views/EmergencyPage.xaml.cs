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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyPage : ContentPage
    {
        public EmergencyPage()
        {
            InitializeComponent();
            BindingContext = new EmergencyChatViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as EmergencyChatViewModel;
            viewModel?.OnAppearingCommand?.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var viewModel = BindingContext as EmergencyChatViewModel;
            viewModel?.OnDisappearingCommand?.Execute(null);
        }
    }
}