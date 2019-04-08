using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public class CopyFileJob : Job
    {
        public CopyFileJob(Client client, MqttMaster channel, 
            IProgress<ProgressDetails> progress)
            : base(client, channel, progress) { }

        public override async Task Work()
        {
            Console.WriteLine("Telling {0} to open NetShare", Client);
            await Channel.PublishAsync("/cmd/" + Client.HostName, "Open Netshare");
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} opened the NetShare", Client);

            Console.WriteLine("Copying File to Netshare");
            await Channel.PublishAsync("/cmd/" + Client.HostName, "Copying File");
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} received the File", Client);

            Console.WriteLine("Telling {0} to close the NetShare", Client);
            await Channel.PublishAsync("/cmd/" + Client.HostName, "Close Netshare");
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} closed the NetShare", Client);

            Console.WriteLine("Telling {0} to Execute the File", Client);
            await Channel.PublishAsync("/cmd/" + Client.HostName, "Execute File");
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} executed the File", Client);
        }

        protected override void AwaitReply(object sender, MqttApplicationMessageReceivedEventArgs args)
        {
            if (args.ApplicationMessage.Topic.Equals("/reply/" + Client.HostName) 
                && args.ApplicationMessage.ConvertPayloadToString().Equals("ACK"))
            {
                Reply.Set();
            }
        }
    }
}