﻿using Android.Bluetooth;
using Java.IO;
using Java.Util;
using RobotApp.Droid.Services.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace RobotApp.Droid.Services
{
    public class BluetoothService : IBluetoothService
    {
        private CancellationTokenSource _ct { get; set; }
        private BluetoothSocket _bluetoothSocket { get; set; }

        public BluetoothService(CancellationTokenSource ct)
        {
            _ct = ct;
        }

        public BluetoothService()
        {
            _ct = new CancellationTokenSource();
        }

        public void StopReading()
        {
            if (_ct != null)
            {
                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }

        /// <summary>
        /// Start the "reading" loop 
        /// </summary>
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        public void StartReading(string name, TimeSpan listeningTime, int sleepTime = 200, bool readAsCharArray = false)
        {
            Task.Run(async () => await Loop(name, sleepTime, readAsCharArray, listeningTime));
        }

        private async Task Loop(string name, int sleepTime, bool readAsCharArray, TimeSpan listeningTime)
        {
            var currentDate = DateTime.Now;

            while (_ct.IsCancellationRequested == false && (DateTime.Now.Subtract(currentDate) < listeningTime))
            {
                try
                {
                    Thread.Sleep(sleepTime);

                    if (_bluetoothSocket.IsConnected)
                    {
                        System.Diagnostics.Debug.WriteLine("Connected!");
                        var mReader = new InputStreamReader(_bluetoothSocket.InputStream);
                        var buffer = new BufferedReader(mReader);

                        while (_ct.IsCancellationRequested == false && (DateTime.Now.Subtract(currentDate) < listeningTime))
                        {
                            if (buffer.Ready())
                            {
                                char[] chr = new char[100];
                                string message = "";

                                if (readAsCharArray)
                                {
                                    await buffer.ReadAsync(chr);
                                    foreach (char c in chr)
                                    {

                                        if (c == '\0')
                                            break;
                                        message += c;
                                    }
                                }
                                else
                                {
                                    message = await buffer.ReadLineAsync();
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("No data to read");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
                    throw;
                }
                finally
                {
                    if (_bluetoothSocket != null)
                    {
                        _bluetoothSocket.Close();
                    }
                }
            }
        }

        public async Task ConnectAsync(string bluetoothName)
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            BluetoothDevice device = null;

            foreach (var bd in adapter.BondedDevices)
            {
                if (bd.Name.ToUpper().IndexOf(bluetoothName.ToUpper()) >= 0)
                {
                    device = bd;
                    break;
                }
            }

            adapter.Dispose();

            if (device == null)
            {
                System.Diagnostics.Debug.WriteLine("Named device not found.");
            }
            else
            {
                UUID uuid = UUID.RandomUUID();
                if ((int)Android.OS.Build.VERSION.SdkInt >= 10)
                {
                    _bluetoothSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                }
                else
                {
                    _bluetoothSocket = device.CreateRfcommSocketToServiceRecord(uuid);
                }

                device.Dispose();

                if (_bluetoothSocket != null)
                {
                    await _bluetoothSocket.ConnectAsync();
                }
            }
        }

        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }

        public void WriteData(string data)
        {
            var javaString = new Java.Lang.String(data);
            var outStream = _bluetoothSocket.OutputStream;

            Java.Lang.String message = javaString;

            byte[] msgBuffer = message.GetBytes();

            try
            {
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error sending message" + e.Message);
                throw;
            }
        }

        public void Close()
        {
            _bluetoothSocket.Dispose();
        }
    }
}