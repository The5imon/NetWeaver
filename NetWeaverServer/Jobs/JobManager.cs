using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.Jobs
{
    public class JobManager
    /**
     * Alle Probleme (Exceptions) die während der Kommunikation auftreten können sollten behandelt werden
     *     - welcher Teil des Programmes sollte welche Exceptions behandeln siehe OSI Layer Model (Transport and Application)
     *     - Welche Fehler können auftreten? (Client disconnected, Timeout, etc ...)
     * ==> Den ganzehn Prozess durchgehen
     *
     */
    //TODO: Deptracated
    {
        private List<string> clients;
        private IProgress<ProgressDetails> progress;
        private ProgressDetails pd = new ProgressDetails();

        public JobManager(List<string> clients, IProgress<ProgressDetails> progress)
        {
            this.clients =  new List<string>(clients);
            this.progress = progress;
        }

        public async Task Work()
        {
            List<Task> jobs = new List<Task>();

            foreach (string client in clients)
            {
                Random r = new Random();
                jobs.Add(Task.Run(() => writeFile(@"data\", client, r.Next(1000) + 1000)));
            }

            await Task.WhenAll(jobs);
        }

        private void writeFile(string path, string client, int count)
        //TODO: Extrat this Task into a Job class with a AutoResetEvent
        {
            for (int i = 0; i < count; i++)
            {
                lock (path) //synchronized area where only 1 process/task can have access
                {
                    File.WriteAllText(path + client + ".txt", client);
                }
            }
            //pd.Clients.Add(client);
            progress.Report(pd);
        }
    }
}