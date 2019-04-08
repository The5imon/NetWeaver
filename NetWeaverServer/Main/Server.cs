using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
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

        //TODO: Rework Event and Handler design
        private async void HandleExecuteScriptEvent(object sender, MessageDetails md)
        {
            await StartJob(typeof(CopyFileJob), md);
        }
        
        private async Task StartJob(Type job, MessageDetails md)
        {
            JobManager manager = new JobManager(job, md, Channel);
            await manager.RunOnAllClients();
        }
    }
}
