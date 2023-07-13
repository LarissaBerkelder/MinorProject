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
    public partial class SavePage : ContentPage
    {
        public SavePage()
        {
            InitializeComponent();
            BindingContext = new SaveViewModel();
        }
    }
}