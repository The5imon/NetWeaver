using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Jobs
{
    public class ExecuteScriptJob : Job
    {
        public ExecuteScriptJob(ClientChannel channel, JobProgress progress, string script)
            : base(channel, progress, script)
        {
            Commands.AddRange(new Job[]
            {
                new ClientJob(Channel, Progress, Cmd.Openshare),
                new CopyFileJob(Channel, Progress, Args),
                new ClientJob(Channel, Progress, Cmd.Closeshare),
                new ClientJob(Channel, Progress, Cmd.Exescript + " " + Args),
            });
        }

        public override async Task Work()
        {
            foreach (Job cmd in Commands)
            {
                await cmd.Work();
            }
        }
    }
}