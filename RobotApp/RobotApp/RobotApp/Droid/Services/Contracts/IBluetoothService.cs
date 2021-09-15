using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RobotApp.Droid.Services.Contracts
{
    public interface IBluetoothService
    {
        void StartReading(string name, TimeSpan listeningTime, int sleepTime, bool readAsCharArray);
        void StopReading();
        void WriteData(string data);
        ObservableCollection<string> PairedDevices();
        Task ConnectAsync(string bluetoothName);
        void Close();
    }
}