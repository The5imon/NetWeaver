using System;
using System.Collections.Generic;
using System.Security.Policy;
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
                using (BerkeleyPacketFilter filter = communicator.CreateFilter("ether[0] & 1 != 0"))
                {
                   communicator.SetFilter(filter);
                }
                
                do
                {
                    Console.WriteLine("new packet");
                    Packet packet;
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);

                    if (result != PacketCommunicatorReceiveResult.Ok) continue;
                    Console.WriteLine(packet.BytesSequenceToHexadecimalString());
                    break;
                } while (true);
            }
        }
    }
}