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
        //TODO: Migrate to complete Commands
        /*
         *  - Jobmanager/TaskManager executes commands on a list of Clients
         *  - Execute CommandSets on Clients
         */
        //TODO: Code a new DeploymentTaskManager
        //TODO: Figure out a better way to generalize Jobs/Commands
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
            await StartJob(typeof(ExecuteScriptJob), task);
        }

        private async Task StartJob(Type job, TaskDetails task)
        {
            JobManager manager = new JobManager(job, task, Channel);
            await manager.RunOnAllClients();
        }
    }
}
