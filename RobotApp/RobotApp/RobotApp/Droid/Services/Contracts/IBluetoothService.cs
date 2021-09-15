using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RobotApp.Droid.Services.Contracts
{
    public interface IBluetoothService
    {
        void StartReading(string name, TimeSpan listeningTime, int sleepTime, bool readAsCharArray);
        void StopReading();
        Task WriteDataAsync(string data);
        ObservableCollection<string> PairedDevices();
        Task<bool> ConnectAsync(string bluetoothName);
        Task<System.Collections.IList> WaitAndReadAsync(TimeSpan? timeOut, string commandName);
        void Close();
    }
}