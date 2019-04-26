using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using Microsoft.SqlServer.Server;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Base;

namespace NetWeaverClient.MQTT
{
    public class DeviceDiscovery
    {
        private readonly string _adapter;
        private readonly string _address;

        public DeviceDiscovery()
        {
            this._adapter = GetDeviceName();
            this._address = GetAddressforDevice();
        }

        private string GetDeviceName()
        {
            string deviceName = string.Empty;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            foreach (var device in allDevices)
            {
                if (device.Description.Contains("Microsoft"))
                {
                    deviceName += device.Description;
                    break;
                }
            }

            return deviceName;
        }

        private string GetAddressforDevice()
        {
            string address = string.Empty;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            foreach (var device in allDevices)
            {
                if (device.Description.Equals(_adapter))
                {
                    foreach (var deviceAddress in device.Addresses)
                    {
                        Console.WriteLine(deviceAddress.Address.ToString());
                        address += deviceAddress.Address.ToString();
                    }
                }
            }

            return address;
        }
    }
}