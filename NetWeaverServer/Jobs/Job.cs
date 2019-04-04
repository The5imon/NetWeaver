using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;

using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Jobs
{
    public abstract class Job
    {
        //TODO: One JobManager (awaits Jobs parallel); Abstract Job class with underlying Jobs (CopyFileJob)
        protected List<Client> Clients { get; }
        protected GUIServerInterface Server { get; }
        protected IProgress<ProgressDetails> Progress { get; }
        protected ProgressDetails Details { get; }

        protected Job(MessageDetails messageDetails, GUIServerInterface server)
        {
            Clients =  new List<Client>(messageDetails.Clients);
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