using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;
using NetWeaverServer.MQTT;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public abstract class Job
    {
        public Client Client { get; }
        protected MqttMaster Channel { get; }
        protected string Topic { get; }
        
        protected AutoResetEvent Reply = new AutoResetEvent(false);
        
        //Used to Reply to the (Task) JobManager
        protected IProgress<JobProgress> JobProgress { get; }

        protected Job(Client client, MqttMaster channel, IProgress<JobProgress> taskProgress)
        {
            Client = client;
            JobProgress = taskProgress;
            Channel = channel;

            Topic = "/cmd/" + Client.HostName;
            Channel.MessageReceivedEvent += AwaitReply;
        }

        public abstract Task Work();

        protected abstract void AwaitReply(object sender, MqttApplicationMessageReceivedEventArgs args);
    }
}