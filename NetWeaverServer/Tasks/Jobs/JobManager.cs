using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.Tasks.Jobs
{
    public class JobManager
    /**
     * Alle Probleme (Exceptions) die während der Kommunikation auftreten können sollten behandelt werden
     *     - welcher Teil des Programmes sollte welche Exceptions behandeln siehe OSI Layer Model (Transport and Application)
     *     - Welche Fehler können auftreten? (Client disconnected, Timeout, etc ...)
     * ==> Den ganzehn Prozess durchgehen
     *
     */
    {
        private List<Client> Clients { get; }
        public IProgress<ProgressDetails> Progress { get; }

        private ProgressDetails pd = new ProgressDetails();

        public JobManager(List<Client> clients, IProgress<ProgressDetails> progress)
        {
            Clients =  new List<Client>(clients);
            Progress = progress;
        }

        public async Task RunOnAllClients()
        {
            List<Task> jobs = new List<Task>();

            foreach (Client client in Clients)
            {
                Random r = new Random();
                jobs.Add(Task.Run(() => writeFile(@"data\", client, r.Next(1000) + 1000)));
            }

            await Task.WhenAll(jobs);
        }
    }
}