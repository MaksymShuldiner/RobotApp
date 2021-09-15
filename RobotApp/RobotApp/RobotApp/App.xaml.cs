using RobotApp.Droid.Services.Contracts;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RobotApp
{
    public partial class App : Application
    {
        public App(IBluetoothService bluetoothService)
        {
            InitializeComponent();

            MainPage = new MainPage(bluetoothService);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
