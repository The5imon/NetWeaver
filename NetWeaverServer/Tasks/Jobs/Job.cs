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
using NetWeaverServer.Tasks.Commands;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public abstract class Job
    {
        /// <summary>
        /// The Client on which Work has to be done
        /// </summary>
        public Client Client { get; }
        protected string Args { get; }

        protected List<ICommand> Commands = new List<ICommand>();

        /// <summary>
        /// Communicate with the Client
        /// </summary>
        protected ClientChannel Channel { get; }

        /// <summary>
        ///Used to Reply to the (Task) JobManager
        /// </summary>
        protected JobProgress Progress { get; }

        protected Job(ClientChannel channel, JobProgress progress, string args)
        {
            Client = channel.Client;
            Args = args;
            Progress = progress;
            Channel = channel;
        }

        /// <summary>
        /// Does the Work defined by the Job
        /// </summary>
        public abstract Task Work();
    }
}