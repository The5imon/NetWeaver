using System.Collections.Generic;

namespace NetWeaverServer.Datastructure.Arguments
{
    public class TaskProgress
    {
        public List<JobProgress> ClientProgress = new List<JobProgress>();

        //NOT quite good
        public void AddJobProgress(JobProgress jp)
        {
            foreach (JobProgress jobProgress in ClientProgress)
            {
                if (jobProgress.Client.Equals(jp.Client))
                {
                    ClientProgress.Remove(jobProgress);
                    break;
                }
            }
            ClientProgress.Add(jp);
        }
    }
}