using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;

namespace NetWeaverServer.Jobs
{
    public abstract class Job
    {
        protected List<string> Clients { get; }
        protected GUIServerInterface Server { get; }
        protected IProgress<ProgressDetails> Progress { get; }
        protected ProgressDetails Details { get; }

        protected Job(MessageDetails messageDetails, GUIServerInterface server)
        {
            Clients =  new List<string>(messageDetails.Clients);
            Server = server;
            Progress = messageDetails.Progress;
            Details = new ProgressDetails();
        }

        public abstract Task Work();

        /**
         * TODO: Working Queue where Jobs just do a minimum set of actions and multiple jobs can be queued together
         */
    }
}