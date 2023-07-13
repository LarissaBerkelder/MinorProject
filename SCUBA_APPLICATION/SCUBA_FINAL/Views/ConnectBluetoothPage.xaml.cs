using SCUBA_FINAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SCUBA_FINAL.Views
{
    public partial class ConnectBluetoothPage : ContentPage
    {
        public ConnectBluetoothPage()
        {
            InitializeComponent();
            BindingContext = new ConnectBluetoothViewModel();
        }
    }
}