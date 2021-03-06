using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Jobs;

namespace NetWeaverServer.Tasks
{
    public class JobManager
    /**
     * Passive: Operation e.g. LoggingOperation
     * Active Job: Job that accomplishes one Action on one Client by executing smaller Commands
     *  --> JobManger that initiates this Job for multiple Clients
     * Active Commands: Do one small thing
     *  --> CommandQueue: Rows nad executes many explicit Commands back to back
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
        /// <summary>
        /// Type of Job that the Clients should work on
        /// </summary>
        private Type Job { get; }
        private List<Client> Clients { get; }
        private string Args { get; }

        /// <summary>
        /// Communicate with the Client
        /// </summary>
        private MqttMaster Mqtt { get; }

        /// <summary>
        /// Means to Communicate/Report back to the GUI
        /// </summary>
        private IProgress<TaskProgress> TaskProgress { get; }
        //Collect Job Reports
        private TaskProgress Progress = new TaskProgress();

        public JobManager(Type job, TaskDetails details, MqttMaster mqtt)
        {
            Job = job;
            Clients =  new List<Client>(details.Clients);
            TaskProgress = details.TaskProgress;
            Args = details.Args;
            Mqtt = mqtt;
        }

        /// <summary>
        /// Runs a Job on all Clients in Paralell
        /// </summary>
        public async Task RunOnAllClients()
        {
            List<Task> tasks = new List<Task>();

            foreach (Client client in Clients)
            {
                //Create a Interface where each Job has a presence in the GUI and can return his Progress
                JobProgress jobProgress = new JobProgress(client);
                jobProgress.ProgressChanged += HandleJobProgressReport;
                Progress.JobProgress.Add(jobProgress);

                //Create new Instance of the specified Job for each Client
                Job j = (Job) Activator.CreateInstance(Job, new ClientChannel(client, Mqtt), jobProgress, Args);
                tasks.Add(j.Work());
            }
            TaskProgress.Report(Progress);
            await Task.WhenAll(tasks);
        }

        private void HandleJobProgressReport(object sender, EventArgs e)
        {
            TaskProgress.Report(Progress);
        }
    }
}