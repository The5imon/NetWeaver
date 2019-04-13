using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PcapDotNet.Core;

namespace NetWeaverClient.MQTT
{
    public class DeviceDiscovery
    {
        private readonly string _adapter;

        public DeviceDiscovery()
        {
            this._adapter = GetDeviceName();
        }

        private string GetDeviceName()
        {
            string deviceName = string.Empty;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            foreach (var device in allDevices)
            {
                deviceName += device.Name + " ";
            }

            return deviceName;
        }
    }
}