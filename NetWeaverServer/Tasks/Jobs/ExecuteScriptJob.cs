using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Jobs
{
    public class ExecuteScriptJob : Job
    {
        private readonly List<Job> Jobs;
        
        public ExecuteScriptJob(Client client, MqttMaster channel, JobProgress progress)
            : base(client, channel, progress)
        {
            //CmdCount = 2;
            Jobs = new List<Job>{ new CopyFileJob(client, channel, progress)};
        }

        public override async Task Work()
        {
            foreach (Job job in Jobs)
            {
                await job.Work();
            }
            
            Console.WriteLine("Telling {0} to Execute the File", Client);
            await Channel.Transmit("Execute File");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} executed the File", Client);
            
            Console.WriteLine("Telling {0} to Execute the Gregor", Client);
            await Channel.Transmit("Execute Gregor");
            Reply.WaitOne();
            Progress.NextCommandDone();
            Console.WriteLine("ACK: {0} executed the Gregor", Client);
            
        }
    }
}