using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;

using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Tasks.Jobs
{
    public abstract class Job
    {
        //TODO: IDEA Three Levels of Jobs:
        /**
         * Passive: Operation e.g. LoggingOperation
         * Active Job: Job that accomplishes one Action on one Client by doing smaller Tasks
         *  --> JobManger that initiates this Job for multiple Clients
         * Active Tasks: Do one small thing
         *  --> TaskQueue: Rows many explicit tasks back to back
         */
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
    }
}