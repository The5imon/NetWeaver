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
        protected JobProgress Progress { get; }

        protected Job(Client client, MqttMaster channel, JobProgress progress)
        {
            Client = client;
            Progress = progress;
            Channel = channel;

            Topic = "/cmd/" + Client.HostName;
            Channel.MessageReceivedEvent += AwaitReply;
        }

        public abstract Task Work();

        protected abstract void AwaitReply(object sender, MqttApplicationMessageReceivedEventArgs args);
    }
}