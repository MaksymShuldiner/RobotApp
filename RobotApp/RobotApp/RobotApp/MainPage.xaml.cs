using RobotApp.Droid.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RobotApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IBluetoothService _bluetoothService;

        public MainPage(IBluetoothService bluetoothService)
        {
            InitializeComponent();
            _bluetoothService = bluetoothService;
            BluetoothPoints.ItemsSource = _bluetoothService.PairedDevices();
            BluetoothPoints.SelectedIndex = 0;
        }

        public async void LoadWifiHotspots(object sender, EventArgs e)
        {
            var bluetoothName = BluetoothPoints.SelectedItem.ToString();

            await _bluetoothService.ConnectAsync(bluetoothName);
            await _bluetoothService.WriteDataAsync("getNetworks");
            var wifiHotspots = await _bluetoothService.WaitAndReadAsync(TimeSpan.FromSeconds(30), "scanResponse");
            WifiPoints.ItemsSource = wifiHotspots;
            _bluetoothService.Close();
        }
    }
}
