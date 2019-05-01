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
        public CopyFileJob(ClientChannel channel, JobProgress progress, string file)
            : base(channel, progress, file)
        {
            Commands.AddRange(new ICommand[]
            {
                new ClientExecute("openshare"),
                new CopyFile(Args),
                new ClientExecute("closeshare"),
            });
            Progress.SetCommandCount(Commands.Count);
        }

        public override async Task Work()
        {
            foreach (ICommand cmd in Commands)
            {
                //Console.WriteLine("Telling {0} to {1}", Client.HostName, cmd);
                await cmd.Execute(Channel);
                Channel.Reply.WaitOne();
                Progress.NextCommandDone();
            }
        }
    }
}