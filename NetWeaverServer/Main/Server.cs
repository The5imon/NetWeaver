using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks;
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
            EventInt.DeploymentEvent += HandleDeploymentEvent;
        }

        private async void HandleDeploymentEvent(object sender, TaskDetails e)
        {
            await new DeploymentManager(e, Channel).DeployForAllClients();
        }

        private async void HandleExecuteScriptEvent(object sender, TaskDetails task)
        {
            await StartJob(typeof(ExecuteScriptJob), task);
        }

        //TODO: Restructure Job Execution; JobManager Concept very vague, lookover required
        //TODO: For general Job execution maybe create JobDetails
        private async Task StartJob(Type job, TaskDetails task)
        {
            await new JobManager(job, task, Channel).RunOnAllClients();
        }
    }
}
