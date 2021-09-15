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
        public MainPage()
        {
            InitializeComponent();
        }

        public async void Message_Sent(object sender, EventArgs e)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 115200;

            if (serialPort.IsOpen)
            {
                string message = this.Message.Text;

                try
                {
                    serialPort.WriteLine(message);
                    await DisplayAlert("Success", "The message was sent", "Ok");
                }
                catch(TimeoutException ex)
                {
                    throw ex;
                }
            }

            serialPort.Close();
        }
    }
}
