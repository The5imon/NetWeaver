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
        /// <summary>
        /// The Client on which Work has to be done
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// Communicate with the Client
        /// </summary>
        protected ClientChannel Channel { get; }
        //To tell the Job when to wait/work
        protected AutoResetEvent Reply = new AutoResetEvent(false);

        /// <summary>
        ///Used to Reply to the (Task) JobManager
        /// </summary>
        protected JobProgress Progress { get; }

        protected Job(Client client, ClientChannel channel, JobProgress progress)
        {
            Client = client;
            Progress = progress;
            Channel = channel;

            Channel.ClientAckEvent += AwaitReply;
        }

        /// <summary>
        /// Does the Work defined by the Job
        /// </summary>
        public abstract Task Work();

        private void AwaitReply(object sender, EventArgs args)
        {
            Reply.Set();
        }
    }
}