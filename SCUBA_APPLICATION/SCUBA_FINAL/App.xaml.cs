using SCUBA_FINAL.Views;
using SCUBA_FINAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.BLE.Abstractions.Contracts;

namespace SCUBA_FINAL
{
    public partial class App : Application
    {
        // Create database for MSG
        static MSG_Database _msg_database; 
        public static MSG_Database MSG_Database
        {
            get
            {
                if (_msg_database == null)
                {
                    _msg_database = new MSG_Database(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MSG-DB.db3"));
                }
                return _msg_database;
            }
        }

        // Create database for EMR
        static EMR_Database _emr_database;
        public static EMR_Database EMR_Database
        {
            get
            {
                if (_emr_database == null)
                {
                    _emr_database = new EMR_Database(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EMR-DB.db3"));
                }
                return _emr_database;
            }
        }

        // Create database for GPB
        static GPB_Database _gpb_database;
        public static GPB_Database GPB_Database
        {
            get
            {
                if (_gpb_database == null)
                {
                    _gpb_database = new GPB_Database(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GPB-DB.db3"));
                }
                return _gpb_database;
            }
        }

        // Globally accessable Characteristic for bluetooth communication
        public ICharacteristic Characteristic { get; set; }

        // Last received message
        private string _prevEMR;
        public string PrevEMR { 
            get 
            { 
                if(_prevEMR == null)
                {
                    return "null";
                }
                return _prevEMR;
            } 
            set { _prevEMR = value; } }

        private string _prevMSG;
        public string PrevMSG
        {
            get
            {
                if (_prevMSG == null)
                {
                    return "null";
                }
                return _prevMSG;
            }
            set { _prevMSG = value; }
        }

        private string _prevGPB;
        public string PrevGPB
        {
            get
            {
                if (_prevGPB == null)
                {
                    return "null";
                }
                return _prevGPB;
            }
            set { _prevGPB = value; }
        }

        // Notification 
        public int EMR_Notification { get; set; }
        public int MSG_Notification { get; set; }

        
        public App()
        {
            InitializeComponent();
            // Setting the start up page
            var navigationPage = new NavigationPage(new ConnectBluetoothPage());
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Clearing the database on start for testing
            MSG_Database.ClearDatabaseAsync();
            EMR_Database.ClearDatabaseAsync();
            GPB_Database.ClearDatabaseAsync();

            // Setting the notifications on 0 
            EMR_Notification = 0;
            MSG_Notification = 0;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}