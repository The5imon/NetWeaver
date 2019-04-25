using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public class CopyFileJob : Job
    {
        public CopyFileJob(Client client, MqttMaster channel, JobProgress progress)
            : base(client, channel, progress)
        {
            Progress.SetCommandCount(4);
        }

        public override async Task Work()
        {
            Console.WriteLine("Telling {0} to open NetShare", Client);
            await Channel.Transmit("Open Netshare");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} opened the NetShare", Client);

            Console.WriteLine("Copying File to Netshare");
            await Channel.Transmit("Copying File");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} received the File", Client);

            Console.WriteLine("Telling {0} to close the NetShare", Client);
            await Channel.Transmit("Close Netshare");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} closed the NetShare", Client);

            Console.WriteLine("Telling {0} to Execute the File", Client);
            await Channel.Transmit("Execute File");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} executed the File", Client);
        }
    }
}