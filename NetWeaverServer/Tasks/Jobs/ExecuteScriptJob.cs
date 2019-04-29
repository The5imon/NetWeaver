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
        private static List<ICommand> Commands = new List<ICommand>(CopyFileJob.Commands)
        {
            new ClientExecute("execscript")
        };

        public ExecuteScriptJob(Client client, MqttMaster channel, JobProgress progress, string script)
            : base(client, channel, progress, script)
        {
            progress.SetCommandCount(Commands.Count);
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