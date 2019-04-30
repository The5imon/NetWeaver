using System;
using System.Collections.Generic;

namespace NetWeaverServer.Datastructure.Arguments
{
    public class TaskDetails : EventArgs
    {
        public List<Client> Clients { get; }

        public string Args { get; }
        
        public IProgress<TaskProgress> TaskProgress;

        public TaskDetails(List<Client> clients, IProgress<TaskProgress> progress, string args)
        {
            Clients = new List<Client>(clients);
            TaskProgress = progress;
            Args = args;
        }
    }
}