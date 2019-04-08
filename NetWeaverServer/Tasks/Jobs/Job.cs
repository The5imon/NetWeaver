using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using NetWeaverServer.Datastructure;
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
        
        protected AutoResetEvent Reply = new AutoResetEvent(false);
        protected IProgress<ProgressDetails> Progress { get; }
        protected ProgressDetails Details = new ProgressDetails();

        protected Job(Client client, MqttMaster channel, IProgress<ProgressDetails> progress)
        {
            Client = client;
            Progress = progress;
            Channel = channel;
            Channel.MessageReceivedEvent += AwaitReply;
        }

        public abstract Task Work();

        protected abstract void AwaitReply(object sender, MqttApplicationMessageReceivedEventArgs args);
    }
}