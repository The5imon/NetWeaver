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
using NetWeaverServer.Tasks.Commands;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public class CopyFileJob : Job
    {
        internal static List<ICommand> Commands = new List<ICommand>
        {
            new ClientExecute("openshare"),
            new ClientExecute("copyfile"),    //Execute Locally
            new ClientExecute("seefile"),
            new ClientExecute("closeshare"),
        };
        
        public CopyFileJob(Client client, MqttMaster channel, JobProgress progress, string file)
            : base(client, channel, progress, file)
        {
            Progress.SetCommandCount(4);
        }

        public override async Task Work()
        {
            foreach (ICommand cmd in Commands)
            {
                Console.WriteLine("Telling {0} to {1}", Client.HostName, cmd);
                await cmd.Execute(Channel);
                Reply.WaitOne();
                Progress.NextCommandDone();
            }
        }
    }
}