using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;

namespace NetWeaverClient.MQTT
{
    public static class DeviceDiscovery
    {
        public static string StartSniffing()
        {
            string returnIntId;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            int deviceIndex = 0;
            
            for (int i = 0; i < allDevices.Count; i++)
            {
                if (!allDevices[i].GetNetworkInterface().Description.Contains("Realtek")) continue;
                deviceIndex = i;
                Console.WriteLine(allDevices[i].Description);
                break;
            }

            PacketDevice selectedDevice = allDevices[deviceIndex];
            using (PacketCommunicator communicator = selectedDevice.Open(
                65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                using (BerkeleyPacketFilter filter = communicator
                    .CreateFilter("ether multicast and ether[20:2] = 0x2000"))
                {
                   communicator.SetFilter(filter);
                }
                
                do
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out var packet);
                    if (result != PacketCommunicatorReceiveResult.Ok) continue;
                    
                    var bytes = packet.Buffer;
                    var interfaceId = new byte[20];
                    for (int i = 363; i < 383; i++)
                    {
                        interfaceId[i - 363] += bytes[i];
                    }

                    returnIntId = Encoding.ASCII.GetString(interfaceId);
                    break;
                } while (true);
            }
            return returnIntId;
        }
    }
}