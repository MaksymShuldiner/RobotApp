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
        }

        public async void Message_Sent(object sender, EventArgs e)
        {
            var bluetoothName = BluetoothName.Text;
            var bluetoothMessage = BluetoothMessage.Text;

            await _bluetoothService.ConnectAsync(bluetoothName);
            _bluetoothService.WriteData(bluetoothMessage);
            _bluetoothService.Close();
        }
    }
}
