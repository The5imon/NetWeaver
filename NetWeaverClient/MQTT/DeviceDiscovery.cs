using System;
using System.Collections.Generic;
using PcapDotNet.Base;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;

namespace NetWeaverClient.MQTT
{
    public class DeviceDiscovery
    {
        public DeviceDiscovery()
        {
            StartSniffing();
        }

        private void StartSniffing()
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            int deviceIndex = 0;
            
            for (int i = 0; i < allDevices.Count; i++)
            {
                if (!allDevices[i].GetNetworkInterface().Description.Contains("Network")) continue;
                deviceIndex = i;
                Console.WriteLine(allDevices[i].Description);
                break;
            }

            PacketDevice selectedDevice = allDevices[deviceIndex];
            using (PacketCommunicator communicator = selectedDevice.Open(
                65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                bool b = true;
                do
                {
                    Packet packet;
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Ok:
                            if (b) // Condition für CDP finden 
                            {
                                //packet decoden hehe :)
                                Console.WriteLine(packet.BytesSequenceToHexadecimalString());
                                
                            }
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                } while (b);
            }
        }
    }
}