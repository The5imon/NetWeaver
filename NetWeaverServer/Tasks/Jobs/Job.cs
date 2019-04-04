using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;

using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public abstract class Job
    {
        public Client Client { get; }
        protected GUIServerInterface Channel { get; }
        protected IProgress<ProgressDetails> Progress { get; }

        protected AutoResetEvent Reply = new AutoResetEvent(false);
        protected event EventHandler<ProgressDetails> JobMessageReceivedEvent;

        protected ProgressDetails Details = new ProgressDetails();

        protected Job(Client client, GUIServerInterface comminication, IProgress<ProgressDetails> progress)
        {
            Client = client;
            Channel = comminication;
            Progress = progress;
            Channel.ClientReplyEvent += AwaitReply;
        }

        public abstract void Work();

        protected abstract void AwaitReply(object sender, MessageDetails args);
    }
}