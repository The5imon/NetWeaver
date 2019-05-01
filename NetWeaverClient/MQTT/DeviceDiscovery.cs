using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Base;
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
                if (!allDevices[i].Description.Equals("Microsoft")) continue;
                deviceIndex = i;
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
                            if (packet.Ethernet.Destination.ToString().Equals("01:00:0c:cc:cc:cc"))
                            {
                                // do something.

                                b = false;
                            }
                            break;
                        case PacketCommunicatorReceiveResult.Timeout:
                            continue;
                        default:
                            throw new InvalidOperationException();
                    }
                } while (b);

                //communicator.ReceivePackets(0, PacketHandler) ;
            }
        }

        /*
        private void PacketHandler(Packet packet)
        {
            if (packet.Ethernet.Destination.ToString().Equals("01:00:0c:cc:cc:cc"))
            {
                Console.WriteLine(packet.Ethernet.Payload.BytesSequenceToHexadecimalString());
            }
        }
        */
    }
}