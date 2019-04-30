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
        public ExecuteScriptJob(ClientChannel channel, JobProgress progress, string script)
            : base(channel, progress, script)
        {
            Commands.Add(new ClientExecute("execscript"));
            Progress.SetCommandCount(Commands.Count);
        }

        public override async Task Work()
        {
            Job j = new CopyFileJob(Channel, Progress, Args);
            await j.Work();
            
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