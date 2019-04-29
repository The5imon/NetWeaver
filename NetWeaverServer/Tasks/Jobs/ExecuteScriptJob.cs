using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Commands;

namespace NetWeaverServer.Tasks.Jobs
{
    public class ExecuteScriptJob : Job
    {
        private ICommand[] Commands =
        {
            new ClientExecute("open netsh"),
            new ClientExecute("copy file"), //Executed Locally
            new ClientExecute("see file?"),
            new ClientExecute("copy file to /scripts"),
            new ClientExecute("close netsh"),
            new ClientExecute("execute file")
        };

        public ExecuteScriptJob(Client client, MqttMaster channel, JobProgress progress)
            : base(client, channel, progress)
        {
            progress.SetCommandCount(Commands.Length);
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