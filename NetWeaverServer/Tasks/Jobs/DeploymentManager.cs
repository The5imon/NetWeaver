using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Jobs
{
    public class DeploymentManager
    {
        private List<Client> Clients { get; }
        private List<Client> Copied { get; }

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

        public DeploymentManager(TaskDetails details, MqttMaster mqtt)
        {
            Clients =  new List<Client>(details.Clients);
            TaskProgress = details.TaskProgress;
            Args = details.Args;
            Mqtt = mqtt;
        }

        /// <summary>
        /// //TODO: Brain me Deloyment
        /// </summary>
        public async Task DeployForAllClients()
        {

            JobProgress jobProgress = new JobProgress(Clients.First());
            jobProgress.ProgressChanged += HandleJobProgressReport;
            Progress.JobProgress.Add(jobProgress);

            await new CopyFileJob(new ClientChannel(Clients.First(), Mqtt), jobProgress, Args).Work();

        }

        private void HandleJobProgressReport(object sender, EventArgs e)
        {
            TaskProgress.Report(Progress);
        }
    }
}