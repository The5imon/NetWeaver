using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Jobs;
using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Main
{
    public class Server
    {
        private EventInterface EventInt { get; }
        private MqttMaster Channel { get; }

        public Server(EventInterface eventInt, MqttMaster channel)
        {
            EventInt = eventInt;
            Channel = channel;
            WireUpHandlers();
        }

        private void WireUpHandlers()
        {
            EventInt.ExecuteScriptEvent += HandleExecuteScriptEvent;
        }

        private async void HandleExecuteScriptEvent(object sender, TaskDetails task)
        {
            await StartJob(typeof(CopyFileJob), task);
        }
        
        private async Task StartJob(Type job, TaskDetails task)
        {
            JobManager manager = new JobManager(job, task, Channel);
            await manager.RunOnAllClients();
        }
    }
}
