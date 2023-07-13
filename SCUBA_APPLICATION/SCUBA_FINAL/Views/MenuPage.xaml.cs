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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new MenuViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as MenuViewModel;
            viewModel?.OnAppearingCommand?.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var viewModel = BindingContext as MenuViewModel;
            viewModel?.OnDisappearingCommand?.Execute(null);
        }

        private void Grid_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}