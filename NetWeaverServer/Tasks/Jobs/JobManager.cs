using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

namespace NetWeaverServer.Tasks.Jobs
{
    public class JobManager
    //TODO: IDEA Three Levels of Jobs:
    /**
     * Passive: Operation e.g. LoggingOperation
     * Active Job: Job that accomplishes one Action on one Client by doing smaller Tasks
     *  --> JobManger that initiates this Job for multiple Clients
     * Active Tasks: Do one small thing
     *  --> TaskQueue: Rows many explicit tasks back to back
     */
    //TODO: Exception Handeling
    /**
     * Alle Probleme (Exceptions) die während der Kommunikation auftreten können sollten behandelt werden
     *     - welcher Teil des Programmes sollte welche Exceptions behandeln siehe OSI Layer Model (Transport and Application)
     *     - Welche Fehler können auftreten? (Client disconnected, Timeout, etc ...)
     * ==> Den ganzen Prozess durchgehen
     *
     */
    {
        private Type Job { get; }
        private List<Client> Clients { get; }
        private GUIServerInterface Channel { get; }
        private IProgress<ProgressDetails> Progress { get; }

        private ProgressDetails pd = new ProgressDetails();

        public JobManager(Type job, MessageDetails messageDetails, GUIServerInterface communication)
        {
            Job = job;
            Clients =  new List<Client>(messageDetails.Clients);
            Progress = messageDetails.Progress;
            Channel = communication;
        }

        public async Task RunOnAllClients()
        {
            List<Task> tasks = new List<Task>();

            foreach (Client client in Clients)
            {
                Job j = (Job) Activator.CreateInstance(Job, client, Channel, Progress);
                tasks.Add(Task.Run(() => j.Work()));
            }

            await Task.WhenAll(tasks);
        }
    }
}